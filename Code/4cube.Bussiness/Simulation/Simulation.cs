using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Data;
using System.Timers;
using _4cube.Bussiness.Config;
using _4cube.Common.Ai;
using _4cube.Common.Components.Crossroad;
using Timer = System.Timers.Timer;

namespace _4cube.Bussiness.Simulation

{
    public class Simulation : ISimulation
    {
        private GridEntity _grid;
        private readonly IConfig _config;
        // private IReport _report;
        private double _time = 0;
        private readonly Timer _timer;
        private SynchronizationContext _uiContext;
        public Simulation(IConfig config)
        {
            _uiContext = SynchronizationContext.Current;
            _timer = new Timer(16);
            _timer.Elapsed += TimerOnElapsed;
            _config = config;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Enabled = false;
            _time++;
            SpawnACar();
            ProcessTrafficLight();
            ProcessCar();
            ProcessPedestrain();
            _timer.Enabled = true;
        }

        private void SpawnACar()
        {
            //Create cars
            foreach (var compo in _grid.Components)
            {
                for (var i = 0; i < compo.NrOfIncomingCars.Length; i++)
                { 
                    if (compo.NrOfIncomingCarsSpawned[i] < compo.NrOfIncomingCars[i])
                    {
                        var direction = (Direction) i;
                        var laneList =
                            _config.GetLanesOfComponent(compo)
                                .Where(x => x.DirectionLane == direction && x.OutgoingDiretion.Any())
                                .ToArray();
                        var rd = new Random();
                        var laneIndex = rd.Next(0, laneList.Count());
                        var laneEnterPoint = laneList[laneIndex].EnterPoint;
                        var laneSpawnPoint = new Tuple<int, int>(laneEnterPoint.Item1 + compo.X,
                            laneEnterPoint.Item2 + compo.Y);
                        var d = _config.CarDistance;
                        var collisionField = new Tuple<int, int, int, int>(laneSpawnPoint.Item1 - d,
                            laneSpawnPoint.Item2 - d, laneSpawnPoint.Item1 + d, laneSpawnPoint.Item2 + d);
                        if (!_grid.Cars.Any(x => collisionField.IsInPosition(x.X, x.Y)))
                        {
                            _grid.Cars.Add(new CarEntity
                            {
                                Direction = direction,
                                X = laneSpawnPoint.Item1,
                                Y = laneSpawnPoint.Item2
                            });
                            compo.NrOfIncomingCarsSpawned[i]++;
                        }
                    }
                }
            }
        }

        public void ChangeSpeed(int n)
        {
            _timer.Interval = n;

        }

        public void Pause()
        {
            _timer.Stop();
        }


        public void Start(GridEntity g)
        {
            _grid = g;
            _timer.Start();
            //TimerOnElapsed(this, null);
        }

        public void Stop()
        {
            _timer.Stop();
            _time = 0;
        }

        private void ProcessTrafficLight()
        {
            var crossroads = _grid.Components.OfType<CrossroadEntity>();
            
            foreach (var c in crossroads)
            {
                if (!(_time >= c.LastTimeSwitched + _grid.GreenLightTimeEntities[c.CurrentGreenLightGroup].Duration))
                    continue;
                var tries = _grid.GreenLightTimeEntities.Count + 1;
                do
                {
                    c.CurrentGreenLightGroup = (c.CurrentGreenLightGroup + 1)%c.GreenLightTimeEntities.Length;
                    var group = c.GreenLightTimeEntities[c.CurrentGreenLightGroup];

                    var cr = _config.CrossRoadCoordinatesCars[group];
                    var pd = _config.CrossRoadCoordinatesPedes.ContainsKey(group) ?_config.CrossRoadCoordinatesPedes[group] : new Tuple<int, int, int, int>[0];

                    if (_config.CrossRoadCoordinatesPedes.ContainsKey(group) && _config.CrossRoadCoordinatesPedes[group].Any())
                    {
                        var rnd = new Random();
                        var number = rnd.Next(1,11);
                        if (number%2 == 0)
                        {
                            //even number means there are pedestrians
                            _grid.Pedestrians.Add(new PedestrianEntity
                            {
                                X = _config.PedstrainSpawn[group].Item1,
                                Y = _config.PedstrainSpawn[group].Item2,
                                Direction = _config.PedstrainSpawn[group].Item3
                            });
                        }
                    }

                    if (_grid.Cars.Any(x=> x.IsInPosition(cr, c.X, c.Y, _config.GridWidth,_config.GridHeight,c.Rotation)) 
                        || _grid.Pedestrians.Any(x => x.IsInPosition(pd, c.X, c.Y, _config.GridWidth, _config.GridHeight, c.Rotation)))
                    {
                        tries = 0;
                    }
                    else
                    {
                        tries--;
                    }
                } while (tries < 0);
            }
        }

        private void MoveCar(CarEntity car)
        {
            var fPos = new Tuple<int, int>(0,0);

            var gridPosition = SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight);
            var component = _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);

            if (component == null)//if car go out of the component
            {
                _grid.Cars.Remove(car);
                return;
            }

            var lanes = _config.GetLanesOfComponent(component);

            var enterLane =
                    lanes.FirstOrDefault(
                        x =>
                            !x.OutgoingDiretion.Any() &&
                            x.BoundingBox.IsInPosition(car.X, car.Y, gridPosition.Item1, gridPosition.Item2, _config.GridWidth,_config.GridHeight, component.Rotation));
            if (enterLane != null)
            {
                fPos = MoveCarToPoint(car, enterLane.ExitPoint);
            }
            else
            {
                var exitLane =
                lanes.FirstOrDefault(
                    x =>
                        x.OutgoingDiretion.Any() &&
                        x.BoundingBox.IsInPosition(car.X, car.Y, gridPosition.Item1, gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation));
                if (exitLane != null)
                {
                    fPos = MoveCarToPoint(car, exitLane.ExitPoint);
                }
                else
                {
                    exitLane =
                        lanes.First(
                            x => !x.OutgoingDiretion.Any() && x.DirectionLane.RotatedDirection(component.Rotation) == car.Direction);
                    fPos = MoveCarToPoint(car, exitLane.EnterPoint);
                }
            }

            // Check if the car is crossing a border of the crossroad
            if (enterLane != null)
            {
                // If the car was first in an entering lane but now not anymore
                if (!enterLane.BoundingBox.IsInPosition(fPos.Item1, fPos.Item2, gridPosition.Item1,
                    gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation))
                {
                    var random = new Random();
                    var i = random.Next(enterLane.OutgoingDiretion.Length);
                    car.Direction = enterLane.OutgoingDiretion[i];
                }
                
            }

            //Check if there is no car at the position the car wants to go to
            var d = _config.CarDistance;
            var collisionField = new Tuple<int,int,int,int>(fPos.Item1 - d, fPos.Item2 - d, fPos.Item1 + d, fPos.Item2 + d);

            // Check if car crosses grid border hence go to other component
            if (!Equals(SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight), SimulationUtility.GetGridPosition(fPos.Item1, fPos.Item2, _config.GridWidth, _config.GridHeight)))
            {
                gridPosition = SimulationUtility.GetGridPosition(fPos.Item1, fPos.Item2, _config.GridWidth,
                    _config.GridHeight);
                if (gridPosition == null)//if car go out of the component
                {
                    _grid.Cars.Remove(car);
                }
                else
                {
                    var l = _config.GetLanesOfComponent(component)
                        .Where(x => x.OutgoingDiretion.Any() && x.DirectionLane.RotatedDirection(component.Rotation) == car.Direction).ToArray();
                    if (!l.Any())
                    {
                        _grid.Cars.Remove(car);
                    }
                    else
                    {
                        var random = new Random();
                        var i = random.Next(l.Count());
                        var lane = l[i];
                        fPos = lane.EnterPoint;//fPos = MoveCarToPoint(car, lane.EnterPoint);
                    }
                }
            }

            if (!_grid.Cars.Where(x => x != car).Any(x => collisionField.IsInPosition(x.X, x.Y)))
            {
                car.X = fPos.Item1;
                car.Y = fPos.Item2;
            }
        }

        public Tuple<int, int> MoveCarToPoint(CarEntity car, Tuple<int, int> point)
        {
            var angle = Math.Atan2(point.Item2 - car.Y, point.Item1 - car.X);
            return new Tuple<int, int>(
                car.X + (int)(Math.Cos(angle) * _config.CarSpeed),
                car.Y + (int)(Math.Sin(angle) * _config.CarSpeed)
                );
        }

        public void MovePedestrain(PedestrianEntity pedestrian)
        {
            switch (pedestrian.Direction)
            {
                case Direction.Up:
                    pedestrian.Y -= _config.PedestrianSpeed;
                    break;
                case Direction.Right:
                    pedestrian.X += _config.PedestrianSpeed;
                    break;
                case Direction.Down:
                    pedestrian.Y += _config.PedestrianSpeed;
                    break;
                case Direction.Left:
                    pedestrian.X -= _config.PedestrianSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessPedestrain()
        {
            var dis = 90;
            var invDis = _config.GridWidth - dis;
            var pedestrains = _grid.Components.OfType<PedestrianEntity>();
            foreach (var p in pedestrains)
            {
                MovePedestrain(p);
                switch (p.Direction)
                {
                    case Direction.Up:
                        if (p.Y < dis)
                        {
                            _grid.Pedestrians.Remove(p);
                        }
                        break;
                    case Direction.Right:
                        if (p.X > invDis)
                        {
                            _grid.Pedestrians.Remove(p);
                        }
                        break;
                    case Direction.Down:
                        if (p.Y > invDis)
                        {
                            _grid.Pedestrians.Remove(p);
                        }

                        break;
                    case Direction.Left:
                        if (p.X < dis)
                        {
                            _grid.Pedestrians.Remove(p);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ProcessCar()
        {

            for (var i = _grid.Cars.Count - 1 ; i >= 0; i--)
            {
                var car = _grid.Cars[i];

                var gridPosition = SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight);
                var component = _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);

                var crossroad = component as CrossroadEntity;
                var road = component as RoadEntity;

                if (crossroad != null)
                {
                    if (car.IsInPosition(_config.GetAllLanesOfTrafficLight(crossroad.GetType()), gridPosition.Item1, gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation)) // is the car in any of the lanes of the crossroad
                    {
                        var trafficlightGroup = crossroad.GreenLightTimeEntities[crossroad.CurrentGreenLightGroup];
                        if (car.IsInPosition(_config.CrossRoadCoordinatesCars[trafficlightGroup], gridPosition.Item1, gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation)) // Light is green of the lane it is tanding in
                            MoveCar(car);
                    }
                    else
                    {
                        MoveCar(car);
                    }
                }
                else if (road != null)
                {
                    MoveCar(car);
                }
            }
        }
    }
}
