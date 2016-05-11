﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Data;
using System.Timers;
using _4cube.Bussiness.Config;
using _4cube.Common.Ai;
using _4cube.Common.Components.Crossroad;

namespace _4cube.Bussiness.Simulation

{
    public class Simulation : ISimulation
    {
        private GridEntity _grid;
        private readonly IConfig _config;
        // private IReport _report;
        private double _time = 0;
        private readonly Timer _timer;
        private int _speed = 2;
        private int _pedestrianSpeed = 1;

        public Simulation(IConfig config)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += TimerOnElapsed;
            _config = config;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _time++;
            ProcessTrafficLight();
            ProcessCar();
            ProcessPedestrain();
        }

        public void ChangeSpeed(double n)
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
                if (!(_time >= c.LastTimeSwitched + c.GreenLightTimeEntities[c.CurrentGreenLightGroup].Duration))
                    continue;
                var tries = c.GreenLightTimeEntities.Count + 1;
                do
                {
                    c.CurrentGreenLightGroup = (c.CurrentGreenLightGroup + 1)%c.GreenLightTimeEntities.Count;
                    var group = c.GreenLightTimeEntities[c.CurrentGreenLightGroup].TrafficLightGroup;
                    var cr = _config.CrossRoadCoordinatesCars[group];
                    var pd = _config.CrossRoadCoordinatesPedes[group];

                    if (_config.CrossRoadCoordinatesPedes[group].Any())
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



                    if (_grid.Cars.Any(x=> x.IsInPosition(cr, c.X, c.Y)) || _grid.Pedestrians.Any(x => x.IsInPosition(pd, c.X, c.Y)))
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
            switch (car.Direction)
            {
                case Direction.Up:
                    fPos = new Tuple<int, int>(car.X, car.Y - _speed);
                    break;
                case Direction.Right:
                    fPos = new Tuple<int, int>(car.X + _speed, car.Y);
                    break;
                case Direction.Down:
                    fPos = new Tuple<int, int>(car.X, car.Y + _speed);
                    break;
                case Direction.Left:
                    fPos = new Tuple<int, int>(car.X - _speed, car.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Check if there is no car at the position the car wants to go to
            var d = _config.CarDistance;
            var collisionField = new Tuple<int,int,int,int>(fPos.Item1 - d, fPos.Item2 - d, fPos.Item1 + d, fPos.Item2 + d);

            if (!_grid.Cars.Where(x => x != car).Any(x => SimulationUtility.IsInPosition(x.X, x.Y, collisionField)))
            {
                car.X = fPos.Item1;
                car.Y = fPos.Item2;
            }
        }

        public void MovePedestrain(PedestrianEntity pedestrian)
        {
            switch (pedestrian.Direction)
            {
                case Direction.Up:
                    pedestrian.Y -= _pedestrianSpeed;
                    break;
                case Direction.Right:
                    pedestrian.X += _pedestrianSpeed;
                    break;
                case Direction.Down:
                    pedestrian.Y += _pedestrianSpeed;
                    break;
                case Direction.Left:
                    pedestrian.X -= _pedestrianSpeed;
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
            var cars = _grid.Components.OfType<CarEntity>();

            foreach (var car in cars)
            {
                var gridPosition = SimulationUtility.GetGridPosition(car.X, car.Y, _config.GridWidth, _config.GridHeight);
                var component = _grid.Components.FirstOrDefault(x => x.X == gridPosition.Item1 && x.Y == gridPosition.Item2);

                var crossroad = component as CrossroadEntity;
                var road = component as RoadEntity;

                if (crossroad != null)
                {
                    if (car.IsInPosition(_config.GetAllLanesOfTrafficLight(crossroad.GetType()), gridPosition.Item1, gridPosition.Item2)) // is the car in any of the lanes of the crossroad
                    {
                        var trafficlightGroup = crossroad.GreenLightTimeEntities[crossroad.CurrentGreenLightGroup].TrafficLightGroup;
                        if (car.IsInPosition(_config.CrossRoadCoordinatesCars[trafficlightGroup], gridPosition.Item1, gridPosition.Item2)) // Light is green of the lane it is tanding in
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
