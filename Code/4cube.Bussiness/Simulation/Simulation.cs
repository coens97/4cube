using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Diagnostics;
=======
using System.ComponentModel;
>>>>>>> origin/master
using System.Linq;
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
        private int speed = 2;

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
                        Random rnd = new Random();
                        int number = rnd.Next(1,11);
                        if (number%2 == 0)
                        {
                            //even number means there are pedestrians
                            _grid.Pedestrians.Add(new PedestrianEntity
                            {
                                X = 95, 
                                Y = 40,
                                Direction = Direction.Left
                            });
                        }
                        else
                        {
                            //there are no pedestrians
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

        private void ProcessCar()
        {
            var cars = _grid.Components.OfType<CarEntity>();

            foreach (var c in cars)
            {
                switch (c.Direction)
                {
                     case  Direction.Up:
                        c.Y += speed;
                        break;
                     case  Direction.Right:
                        c.X += speed;
                        break;
                     case  Direction.Down:
                        c.Y -= speed;
                        break;
                     case  Direction.Left:
                        c.X -= speed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
