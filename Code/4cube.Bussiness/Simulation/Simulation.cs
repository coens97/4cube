using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using System.Timers;
using _4cube.Bussiness.Config;
using _4cube.Common.Ai;
using _4cube.Common.Components.Crossroad;
using Timer = System.Timers.Timer;

namespace _4cube.Bussiness.Simulation

{
    public class Simulation : ISimulation
    {
        private ReaderWriterLockSlim _rwCar = new ReaderWriterLockSlim();
        private GridEntity _grid;
        private readonly IConfig _config;
        // private IReport _report;
        private int _time = 0;
        private readonly Timer _timer;
        private SynchronizationContext _uiContext;
        private List<CarEntity> _carsToGetDeleted = new List<CarEntity>();
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

            _uiContext.Send(x =>
            {
                _rwCar.EnterWriteLock();
                foreach (var car in _carsToGetDeleted)
                {
                    _grid.Cars.Remove(car);
                }
                
                _rwCar.ExitWriteLock();
                _carsToGetDeleted.Clear();
            }, null);
            
            //ProcessPedestrain();
            _timer.Enabled = true;
        }

        private void SpawnACar()
        {
            //Create cars       
            Parallel.ForEach(_grid.Components.AsParallel(),
                  //new ParallelOptions() { MaxDegreeOfParallelism = 4 }, 
                  (compo =>
            {
                for (var i = 0; i < compo.NrOfIncomingCars.Length; i++)
                {
                    if (compo.NrOfIncomingCarsSpawned[i] >= compo.NrOfIncomingCars[i]) continue;
                    var direction =
                        ((Direction) i).RotatedDirection(compo.Rotation.RotatedDirectionInv(Direction.Left));
                    var laneList =
                        _config.GetLanesOfComponent(compo)
                            .Where(x => x.DirectionLane == direction && x.OutgoingDirection.Any())
                            .ToArray();

                    if (!laneList.Any())
                        continue;

                    var rd = new Random();
                    var laneIndex = rd.Next(0, laneList.Count());
                    var laneEnterPoint = laneList[laneIndex].EnterPoint.Rotate(compo.Rotation, _config.GridWidth,
                        _config.GridHeight);
                    var laneSpawnPoint = new Tuple<int, int>(laneEnterPoint.Item1 + compo.X,
                        laneEnterPoint.Item2 + compo.Y);
                    var d = _config.CarDistance;
                    var collisionField = new Tuple<int, int, int, int>(laneSpawnPoint.Item1 - d,
                        laneSpawnPoint.Item2 - d, laneSpawnPoint.Item1 + d, laneSpawnPoint.Item2 + d);

                    _uiContext.Send(a =>
                    {
                        // only spawn cars when there is position
                        _rwCar.EnterWriteLock();
                        var collision = _grid.Cars.Any(x => collisionField.IsInPosition(x.X, x.Y));
                        if (collision)
                        {
                            _rwCar.ExitWriteLock();
                            return;
                        }
                        _grid.Cars.Add(new CarEntity
                        {
                            Direction = direction.RotatedDirection(compo.Rotation),
                            X = laneSpawnPoint.Item1,
                            Y = laneSpawnPoint.Item2
                        });
                        _rwCar.ExitWriteLock();
                        compo.NrOfIncomingCarsSpawned[i]++;
                    }, null);
                }
            }));
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
        }

        public void Stop()
        {
            _timer.Stop();
            _time = 0;
        }

        private void ProcessTrafficLight()
        {
            var crossroads = _grid.Components.OfType<CrossroadEntity>().ToArray();

            crossroads.AsParallel().ForAll(c =>
            {
                if (_time <= c.LastTimeSwitched + _grid.GreenLightTimeEntities[c.CurrentGreenLightGroup].Duration)
                    return;

                if (!c.LightOrange) // change color of traffic lights to arrange
                {
                    c.LightOrange = true;
                    return;
                }

                if (
                    _grid.Cars.Any(
                        x =>
                            _config.TrafficCenter.IsInPosition(x.X, x.Y, c.X, c.Y, _config.GridWidth, _config.GridHeight,
                                c.Rotation)))
                {
// if there are any cars in the center of the crossroad let the lights stay orange
                    return;
                }
                c.LightOrange = false;
                c.LastTimeSwitched = _time;
                var tries = _grid.GreenLightTimeEntities.Count;
                while (tries > 0)
                {
                    c.CurrentGreenLightGroup = (c.CurrentGreenLightGroup + 1)%c.GreenLightTimeEntities.Length;
                    var group = c.GreenLightTimeEntities[c.CurrentGreenLightGroup];

                    var cr = _config.CrossRoadCoordinatesCars[group].Select(x => x.ExitBounding).ToArray();
                    var pd = _config.CrossRoadCoordinatesPedes.ContainsKey(group)
                        ? _config.CrossRoadCoordinatesPedes[group].Select(x => x.BoundingBox).ToArray()
                        : null;

                    if (_config.CrossRoadCoordinatesPedes.ContainsKey(group) &&
                        _config.CrossRoadCoordinatesPedes[group].Any())
                    {
                        var rnd = new Random();
                        var number = rnd.Next(1, 11);
                        if (number%2 == 0)
                        {
                            //even number means there are pedestrians
                            //_grid.Pedestrians.Add(new PedestrianEntity
                            //{
                            //    X = _config.PedstrainSpawn[group].Item1,
                            //    Y = _config.PedstrainSpawn[group].Item2,
                            //    Direction = _config.PedstrainSpawn[group].Item3
                            //});
                        }
                    }

                    if (_grid.Cars.Any(
                        x => x.IsInPosition(cr, c.X, c.Y, _config.GridWidth, _config.GridHeight, c.Rotation))
                        ||
                        _grid.Pedestrians.Any(
                            x => x.IsInPosition(pd, c.X, c.Y, _config.GridWidth, _config.GridHeight, c.Rotation)))
                    {
                        tries = 0;
                    }
                    else
                    {
                        tries--;
                    }
                }
            });
        }

        private void DeleteCar(CarEntity car)
        {
            _carsToGetDeleted.Add(car);
        }

        private void MoveCar(CarEntity car)
        {
            var fPos = new Tuple<int, int>(0,0);

            var gridPosition = SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight);
            var component = _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);

            if (component == null)//if car go out of the component
            {
                DeleteCar(car);
                return;
            }

            var lanes = _config.GetLanesOfComponent(component);

            var enterLane =
                    lanes.FirstOrDefault(
                        x =>
                            x.OutgoingDirection.Any() &&
                            x.BoundingBox.IsInPosition(car.X, car.Y, gridPosition.Item1, gridPosition.Item2, _config.GridWidth,_config.GridHeight, component.Rotation));

            if (enterLane != null) // if car is in an entering lane
            {
                fPos = MoveCarToPoint(car, enterLane.ExitPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight), component);

                // If the car was first in an entering lane but now not anymore
                if (!enterLane.BoundingBox.IsInPosition(fPos.Item1, fPos.Item2, gridPosition.Item1,
                    gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation))
                {
                    var random = new Random();
                    var i = random.Next(enterLane.OutgoingDirection.Length);
                    car.Direction = enterLane.OutgoingDirection[i].RotatedDirection(component.Rotation);
                }
            }
            else
            {
                var exitLane =
                        lanes.First(
                            x => !x.OutgoingDirection.Any() && x.DirectionLane.RotatedDirection(component.Rotation) == car.Direction);
                if (exitLane.BoundingBox.IsInPosition(car.X, car.Y, gridPosition.Item1, gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation))
                {//if car is leaving the exit lane
                        fPos = MoveCarToPoint(car, exitLane.ExitPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight), component);
                }
                else
                {//if car is going to the exit lane
                    fPos = MoveCarToPoint(car,
                        exitLane.EnterPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight), component);
                }
            }

            //Check if there is no car at the position the car wants to go to
            var d = _config.CarDistance;
            
            // Check if car crosses grid border hence go to other component
            if (!Equals(SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight), SimulationUtility.GetGridPosition(fPos.Item1, fPos.Item2, _config.GridWidth, _config.GridHeight)))
            {
                gridPosition = SimulationUtility.GetGridPosition(fPos.Item1, fPos.Item2, _config.GridWidth,
                    _config.GridHeight);
                var nextComponent =
                    _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);
                if (nextComponent == null)//if car go out of the component
                {
                    DeleteCar(car);
                }
                else
                {
                    var l = _config.GetLanesOfComponent(nextComponent)
                        .Where(x => x.OutgoingDirection.Any() && x.DirectionLane.RotatedDirection(nextComponent.Rotation) == car.Direction).ToArray();
                    if (!l.Any())
                    {
                        DeleteCar(car);
                    }
                    else
                    {
                        var random = new Random();
                        var i = random.Next(l.Length);
                        var lane = l[i].EnterPoint.Rotate(nextComponent.Rotation, _config.GridWidth, _config.GridHeight);
                        fPos = new Tuple<int, int>(lane.Item1 + nextComponent.X, lane.Item2 + nextComponent.Y);
                    }
                }
            }

            var collisionField = new Tuple<int, int, int, int>(fPos.Item1 - d, fPos.Item2 - d, fPos.Item1 + d, fPos.Item2 + d);

            var collision = _grid.Cars.Any(x =>  x != car && collisionField.IsInPosition(x.X, x.Y));

            if (collision) return;

            car.X = fPos.Item1;
            car.Y = fPos.Item2;
        }

        public Tuple<int, int> MoveCarToPoint(CarEntity car, Tuple<int, int> point, ComponentEntity component)
        {
            var angle = Math.Atan2((component.Y + point.Item2) - car.Y, (component.X + point.Item1) - car.X);
            var fpos = new Tuple<int, int>(
                car.X + (int)(Math.Cos(angle) * _config.CarSpeed),
                car.Y + (int)(Math.Sin(angle) * _config.CarSpeed)
                );
            if (fpos.Item1 == component.X + point.Item1 && fpos.Item2 == component.Y + point.Item2) // check if the future position is not the same as the destination
                fpos = new Tuple<int, int>( // give a little boost
                    car.X + (int)(Math.Cos(angle) * _config.CarSpeed * 1.5),
                    car.Y + (int)(Math.Sin(angle) * _config.CarSpeed * 1.5)
                    );
            return fpos;
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
            _grid.Cars.ToArray().AsParallel().ForAll(car =>
            {
                var gridPosition = SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight);
                var component =
                    _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);

                var crossroad = component as CrossroadEntity;
                var road = component as RoadEntity;

                if (crossroad != null)
                {
                    var lanes = _config.GetLanesOfComponent(crossroad);
                    var incomingLanes = lanes.Where(x => x.OutgoingDirection.Any());
                    var incomingLane =
                        incomingLanes.FirstOrDefault(
                            x =>
                                x.ExitBounding.IsInPosition(car.X, car.Y, component.X, component.Y, _config.GridWidth,
                                    _config.GridHeight, component.Rotation));
                    if (incomingLane != null)
                        // is the car in any of the lanes of the crossroad
                    {
                        var outgoingLanes = lanes.Where(x => !x.OutgoingDirection.Any()
                                                             &&
                                                             incomingLane.OutgoingDirection.Any(
                                                                 y => x.DirectionLane == y));
                        var trafficlightGroup = crossroad.GreenLightTimeEntities[crossroad.CurrentGreenLightGroup];
                        var sensors =
                            _config.CrossRoadCoordinatesCars[trafficlightGroup].Select(x => x.ExitBounding).ToArray();

                        var carInOutgoingLane = !_grid.Cars.Any(
                            c => outgoingLanes.Select(x => x.ExitBounding).ToArray()
                                .IsInPosition(c.X, c.Y, component.X, component.Y, _config.GridWidth,
                                    _config.GridHeight, component.Rotation));

                        if (carInOutgoingLane && !crossroad.LightOrange &&
                            car.IsInPosition(sensors, gridPosition.Item1,
                                gridPosition.Item2, _config.GridWidth, _config.GridHeight, component.Rotation))
                            // Light is green of the lane it is standing in
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
            });
        }
    }
}
