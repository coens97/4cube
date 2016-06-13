using System;
using System.Collections.Concurrent;
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
        private GridEntity _grid;
        private readonly IConfig _config;
        // private IReport _report;
        private int _time = 0;
        private bool _enabled = false;
        private bool _stop = false;
        private readonly Timer _timer;
        private bool _constantCalculation = false;
        private readonly SynchronizationContext _uiContext;

        private readonly ConcurrentQueue<CarEntity> _carsToGetAdded = new ConcurrentQueue<CarEntity>();
        private readonly ConcurrentQueue<CarEntity> _carsToGetDeleted = new ConcurrentQueue<CarEntity>();
        private readonly ConcurrentQueue<PedestrianEntity> _pedestriansToGetAdded = new ConcurrentQueue<PedestrianEntity>();
        private readonly ConcurrentQueue<PedestrianEntity> _pedestriansToGetDeleted = new ConcurrentQueue<PedestrianEntity>();
        public Simulation(IConfig config)
        {
            _uiContext = SynchronizationContext.Current;
            _timer = new Timer(15);
            _timer.Elapsed += TimerOnElapsed;
            _config = config;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Enabled = false;

            if (!_enabled)
            {
                if (!_stop) return;

                _stop = false;
                _time = 0;
                // reset everything
                _uiContext.Send(x =>
                {
                    _grid.Cars.Clear();
                    _grid.Pedestrians.Clear();
                }, null);

                _grid.Components.AsParallel().ForAll(x =>
                {
                    x.CarsInComponentLock.EnterWriteLock();
                    x.CarsInComponent.Clear();
                    x.CarsInComponentLock.ExitWriteLock();
                    x.NrOfIncomingCarsSpawned = new[] { 0, 0, 0, 0 };
                });
                return;
            }

            do
            {
                _time++;
                SpawnACar();
                ProcessTrafficLight();
                ProcessPedestrian();
                ProcessCar();

                _uiContext.Send(x => // this will run in the UI thread
                {
                    CarEntity car;
                    while(_carsToGetAdded.TryDequeue(out car))
                    {
                        _grid.Cars.Add(car);
                    }
                    while (_carsToGetDeleted.TryDequeue(out car))
                    {
                        _grid.Cars.Remove(car);
                    }
                    PedestrianEntity ped;
                    while (_pedestriansToGetAdded.TryDequeue(out ped))
                    {
                        _grid.Pedestrians.Add(ped);
                    }
                    while (_pedestriansToGetDeleted.TryDequeue(out ped))
                    {
                        _grid.Pedestrians.Remove(ped);
                    }
                }, null);
            } while (_constantCalculation && _enabled);

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
                            ((Direction)i).RotatedDirection(compo.Rotation.RotatedDirectionInv(Direction.Left));
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

                        var car = new CarEntity
                        {
                            Direction = direction.RotatedDirection(compo.Rotation),
                            X = laneSpawnPoint.Item1,
                            Y = laneSpawnPoint.Item2
                        };

                        if (CheckCarCollision(compo, car, new Tuple<int, int>(car.X, car.Y)))
                        {
                            return;
                        }
                        compo.CarsInComponent.Add(car);
                        _carsToGetAdded.Enqueue(car);
                        compo.NrOfIncomingCarsSpawned[i]++;
                    }
                }));
        }

        public void ChangeSpeed(int n)
        {
            if (n == 10)
            {
                _constantCalculation = true;
            }
            else
            {
                _constantCalculation = false;
                _timer.Interval = (9 - n) * 4 + 16;
            }

        }

        public void Pause()
        {
            _enabled = false;
        }


        public void Start(GridEntity g)
        {
            _grid = g;
            _enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            _enabled = false;
            _stop = true;
        }

        private void SpawnPedestrian(CrossroadBEntity c, IEnumerable<Lane> lanes)
        {
            foreach (var lane in lanes)
            {
                var rnd = new Random();
                var number = rnd.Next(1, 11);
                if (number%2 != 0) continue;
                //even number means there are pedestrians

                var spawn = lane.EnterPoint.Rotate(c.Rotation, _config.GridWidth, _config.GridHeight);
                var ped = new PedestrianEntity
                {
                    X = spawn.Item1 + c.X,
                    Y = spawn.Item2 + c.Y,
                    Direction = lane.DirectionLane.RotatedDirection(c.Rotation)
                };
                c.PedestriansInComponentLock.EnterWriteLock();
                c.PedestriansInComponent.Add(ped);
                c.PedestriansInComponentLock.ExitWriteLock();
                _pedestriansToGetAdded.Enqueue(ped);
            }
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

                c.CarsInComponentLock.EnterReadLock();
                var stayOrange = c.CarsInComponent.Any(
                    x =>
                        _config.TrafficCenter.IsInPosition(x.X, x.Y, c.X, c.Y, _config.GridWidth, _config.GridHeight,
                            c.Rotation));
                c.CarsInComponentLock.ExitReadLock();

                if (stayOrange)
                {
                    return;
                }

                var crossroadB = c as CrossroadBEntity;
                if (crossroadB != null)
                {
                    crossroadB.PedestriansInComponentLock.EnterReadLock();
                    stayOrange = crossroadB.PedestriansInComponent.Any();
                    crossroadB.PedestriansInComponentLock.ExitReadLock();
                    if (stayOrange) // stay orange light when there are any pedestrians on the component
                        return;
                }

                c.LightOrange = false;
                c.LastTimeSwitched = _time;
                var tries = _grid.GreenLightTimeEntities.Length;
                while (tries > 0)
                {
                    c.CurrentGreenLightGroup = (c.CurrentGreenLightGroup + 1) % c.GreenLightTimeEntities.Length;
                    var group = c.GreenLightTimeEntities[c.CurrentGreenLightGroup];

                    var cr = _config.CrossRoadCoordinatesCars[group].Select(x => x.ExitBounding).ToArray();
                    var pd = _config.CrossRoadCoordinatesPedes.ContainsKey(group)
                        ? _config.CrossRoadCoordinatesPedes[group].Select(x => x.BoundingBox).ToArray()
                        : null;

                    Lane[] lanes;
                    var crossB = c as CrossroadBEntity;
                    if (crossB != null && _config.CrossRoadCoordinatesPedes.TryGetValue(group, out lanes))
                    {
                        SpawnPedestrian(crossB, lanes);
                    }
                    

                    c.CarsInComponentLock.EnterReadLock();
                    var carsonSensor = c.CarsInComponent.Any(
                        x => x.IsInPosition(cr, c.X, c.Y, _config.GridWidth, _config.GridHeight, c.Rotation));
                    c.CarsInComponentLock.ExitReadLock();

                    var pedesOnSensor = false;
                    if (crossB != null && pd != null)
                    {
                        crossB.PedestriansInComponentLock.EnterReadLock();
                        pedesOnSensor = crossB.PedestriansInComponent.Any(
                            x => x.IsInPosition(pd, c.X, c.Y, _config.GridWidth, _config.GridHeight, c.Rotation));
                        crossB.PedestriansInComponentLock.ExitReadLock();
                    }

                    if (carsonSensor || pedesOnSensor)
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

        private void DeleteCar(CarEntity car, ComponentEntity component)
        {
            _carsToGetDeleted.Enqueue(car);
            component?.CarsInComponentLock.EnterWriteLock(); // Component could be deleted before cars get deleted
            component?.CarsInComponent.Remove(car);
            component?.CarsInComponentLock.ExitWriteLock();
        }

        private bool CheckCarCollision(ComponentEntity component, CarEntity car, Tuple<int, int> fPos)
        {
            var d = _config.CarDistance;
            var collisionField = new Tuple<int, int, int, int>(fPos.Item1 - d, fPos.Item2 - d, fPos.Item1 + d, fPos.Item2 + d);
            component.CarsInComponentLock.EnterReadLock();
            var collision = component.CarsInComponent.Any(x => x != car && collisionField.IsInPosition(x.X, x.Y));
            component.CarsInComponentLock.ExitReadLock();
            return collision;
        }

        private Tuple<int, int> MoveCarEnterLane(ComponentEntity component, CarEntity car, Lane enterLane)
        {
            var fPos = MoveCarToPoint(car, enterLane.ExitPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight), component, _config.CarSpeed);

            if (enterLane.BoundingBox.IsInPosition(fPos.Item1, fPos.Item2, component.X,
                component.Y, _config.GridWidth, _config.GridHeight, component.Rotation)) return fPos;

            // If the car was first in an entering lane but now not anymore
            var random = new Random();
            var i = random.Next(enterLane.OutgoingDirection.Length);
            car.Direction = enterLane.OutgoingDirection[i].RotatedDirection(component.Rotation);
            return fPos;
        }

        private Tuple<int, int> MoveCarExitLane(ComponentEntity component, CarEntity car, Lane exitLane)
        {
            var exit = exitLane.ExitPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight);
            var fPos = MoveCarToPoint(car, exit, component, _config.CarSpeed);

            var currentGrid = new Tuple<int, int>(component.X, component.Y);

            var nextGrid = SimulationUtility.GetGridPosition(fPos.Item1, fPos.Item2, _config.GridWidth,
                _config.GridHeight);


            var nextComponent =
                _grid.Components.FirstOrDefault(x => x.X == nextGrid.Item1 && x.Y == nextGrid.Item2);


            if (currentGrid.Equals(nextGrid)) return fPos;

            if (nextComponent == null) //if car go out of the component
            {
                DeleteCar(car, component);
                return fPos;
            }

            var l = _config.GetLanesOfComponent(nextComponent)
                .Where(
                    x =>
                        x.OutgoingDirection.Any() &&
                        x.DirectionLane.RotatedDirection(nextComponent.Rotation) == car.Direction)
                .ToArray();

            if (!l.Any())// if there are no entering lanes destroy the car
            {
                DeleteCar(car, component);
                return fPos;
            }
            // swich car to other component
            var random = new Random();
            var i = random.Next(l.Length);
            var lane = l[i].EnterPoint.Rotate(nextComponent.Rotation, _config.GridWidth,
                _config.GridHeight);
            fPos = new Tuple<int, int>(lane.Item1 + nextComponent.X,
                lane.Item2 + nextComponent.Y);

            if (CheckCarCollision(nextComponent, car, fPos)) return new Tuple<int, int>(car.X, car.Y);

            component.CarsInComponentLock.EnterWriteLock();
            component.CarsInComponent.Remove(car);
            component.CarsInComponentLock.ExitWriteLock();
            nextComponent.CarsInComponentLock.EnterWriteLock();
            nextComponent.CarsInComponent.Add(car);
            nextComponent.CarsInComponentLock.ExitWriteLock();
            car.X = fPos.Item1;
            car.Y = fPos.Item2;
            return fPos;
        }

        private void MoveCar(ComponentEntity component, CarEntity car, Lane enterLane, Lane exitLane)
        {
            Tuple<int, int> fPos;
            
            if (enterLane != null) // if car is in an entering lane
            {
                fPos = MoveCarEnterLane(component, car, enterLane);
            }
            else if (exitLane != null)
            {
                if (exitLane.BoundingBox.IsInPosition(car.X, car.Y, component.X, component.Y, _config.GridWidth, _config.GridHeight, component.Rotation))
                {//if car is leaving the exit lane
                    fPos = MoveCarExitLane(component, car, exitLane);
                }
                else
                {//if car is going to the exit lane
                    fPos = MoveCarToPoint(car,
                        exitLane.EnterPoint.Rotate(component.Rotation, _config.GridWidth, _config.GridHeight), component, _config.CarSpeed);
                }
            }
            else
            {
                return;
            }

            if (CheckCarCollision(component, car, fPos)) return;

            car.X = fPos.Item1;
            car.Y = fPos.Item2;
        }

        public static Tuple<int, int> MoveCarToPoint(CarEntity car, Tuple<int, int> point, ComponentEntity component, int distance)
        {
            var angle = Math.Atan2((component.Y + point.Item2) - car.Y, (component.X + point.Item1) - car.X);
            var fpos = new Tuple<int, int>(
                car.X + (int)(Math.Cos(angle) * distance),
                car.Y + (int)(Math.Sin(angle) * distance)
                );
            if (fpos.Item1 == component.X + point.Item1 && fpos.Item2 == component.Y + point.Item2) // check if the future position is not the same as the destination
                fpos = new Tuple<int, int>( // give a little boost
                    car.X + (int)(Math.Cos(angle) * distance * 1.5),
                    car.Y + (int)(Math.Sin(angle) * distance * 1.5)
                    );
            return fpos;
        }

        public void MovePedestrian(PedestrianEntity pedestrian, Direction direction)
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

        private void RemovePedestrian(CrossroadBEntity c, PedestrianEntity p)
        {
            c.PedestriansInComponentLock.EnterWriteLock();
            c.PedestriansInComponent.Remove(p);
            c.PedestriansInComponentLock.ExitWriteLock();
            _pedestriansToGetDeleted.Enqueue(p);
        }

        private void ProcessPedestrian()
        {
            var dis = 90;
            var invDis = _config.GridWidth - dis;
            _grid.Components.OfType<CrossroadBEntity>().AsParallel().ForAll(c =>
            {
                for (var i = c.PedestriansInComponent.Count - 1; i >= 0; i--)
                {
                    var p = c.PedestriansInComponent[i];
                    MovePedestrian(p, c.Rotation);
                    switch (p.Direction)
                    {
                        case Direction.Up:
                            if (p.Y < dis + c.Y)
                            {
                                RemovePedestrian(c, p);
                            }
                            break;
                        case Direction.Right:
                            if (p.X > invDis + c.X)
                            {
                                RemovePedestrian(c, p);
                            }
                            break;
                        case Direction.Down:
                            if (p.Y > invDis + c.Y)
                            {
                                RemovePedestrian(c, p);
                            }

                            break;
                        case Direction.Left:
                            if (p.X < dis + c.X)
                            {
                                RemovePedestrian(c, p);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        private void CheckCanMoveCar(ComponentEntity component, CarEntity car)
        {
            if (car == null)
                return;

            var crossroad = component as CrossroadEntity;

            var lanes = _config.GetLanesOfComponent(component);
            var incomingLanes = lanes.Where(x => x.OutgoingDirection.Any());
            var incomingLane =
                    incomingLanes.FirstOrDefault(
                        x =>
                            x.BoundingBox.IsInPosition(car.X, car.Y, component.X, component.Y, _config.GridWidth,
                                _config.GridHeight, component.Rotation));

            IEnumerable<Lane> outgoingLanes = null;
            Lane exitLane = null;
            if (incomingLane != null) // if the car is not on a incominglane then get the outgoing lane
            {
                outgoingLanes = lanes.Where(x => !x.OutgoingDirection.Any()
                                                     &&incomingLane.OutgoingDirection.Any(y => x.DirectionLane == y));
            }
            else
            {
                exitLane =
                        lanes.FirstOrDefault(
                            x => !x.OutgoingDirection.Any() && x.DirectionLane.RotatedDirection(component.Rotation) == car.Direction);
            }

            if (crossroad != null)
            {
                if (incomingLane != null)
                // is the car in any of the lanes of the crossroad
                {
                    var trafficlightGroup = crossroad.GreenLightTimeEntities[crossroad.CurrentGreenLightGroup];
                    
                    if (incomingLane.ExitBounding.IsInPosition(car.X, car.Y, component.X,
                        component.Y, _config.GridWidth, _config.GridHeight, component.Rotation)) // if car is on the exit sensor
                    {
                        if (crossroad.LightOrange)
                            return;

                        if (!_config.CrossRoadCoordinatesCars[trafficlightGroup].Contains(incomingLane)) // if light is not green
                            return;

                        component.CarsInComponentLock.EnterReadLock();
                        var carInOutgoingLane = component.CarsInComponent.Any(
                            c => outgoingLanes.Select(x => x.ExitBounding).ToArray()
                                .IsInPosition(c.X, c.Y, component.X, component.Y, _config.GridWidth,
                                    _config.GridHeight, component.Rotation));
                        component.CarsInComponentLock.ExitReadLock();

                        if (carInOutgoingLane)//if any cars in outgoing lane
                            return;
                    }
                }
                //var crossB = component as CrossroadBEntity;
                //if (crossB != null) // on crossroad b cars can't stop on the walking lane
                //{
                //    var lane = incomingLane ?? exitLane;
                //    Lane walkLane;
                //    if (_config.CrossCarLanePedesLane.TryGetValue(lane, out walkLane) && lane.EnterBounding.IsInPosition(car.X, car.Y, component.X,
                //        component.Y, _config.GridWidth, _config.GridHeight, component.Rotation))
                //    {
                //        component.CarsInComponentLock.EnterReadLock();
                //        var carOnWalkSensors = component.CarsInComponent.Any(
                //            c => c != car &&
                //                walkLane.BoundingBox.IsInPosition(car.X, car.Y, component.X, component.Y,
                //                    _config.GridWidth,
                //                    _config.GridHeight, component.Rotation, _config.CarDistance) &&
                //                lane.BoundingBox.IsInPosition(c.X, c.Y, component.X, component.Y,
                //                    _config.GridWidth,
                //                    _config.GridHeight, component.Rotation, _config.CarDistance));
                //        component.CarsInComponentLock.ExitReadLock();
                //        if (carOnWalkSensors)
                //            return;
                //    }
                //}
            }
            MoveCar(component, car, incomingLane, exitLane);
        }

        private void ProcessCar()
        {
            _grid.Components.AsParallel().ForAll(c =>
            {
                for (var i = c.CarsInComponent.Count - 1; i >= 0; i--)
                {
                    CheckCanMoveCar(c, c.CarsInComponent[i]);
                }
            });
        }
    }
}
