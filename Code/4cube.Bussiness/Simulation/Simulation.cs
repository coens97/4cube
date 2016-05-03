using System;
using System.Collections.Generic;
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
        private IConfig _config;
        // private IReport _report;
        private double _time = 0;
        private Timer _timer;

        public Simulation(IConfig config)
        {
            _timer = new Timer(_time);
            _timer.Elapsed += TimerOnElapsed;
            _config = config;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _time++;
            ProcessTrafficLight();
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
            TimerOnElapsed(this, null);
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

                if (_time >= c.LastTimeSwitched + c.GreenLightTimeEntities[c.CurrentGreenLightGroup].Duration)
                {
                    int tries = c.GreenLightTimeEntities.Count + 1;
                    do
                    {
                        c.CurrentGreenLightGroup = (c.CurrentGreenLightGroup + 1)%c.GreenLightTimeEntities.Count;
                        var group = c.GreenLightTimeEntities[c.CurrentGreenLightGroup].TrafficLightGroup;
                        var cr = _config.CrossRoadCoordinatesCars[group];
                        var pd = _config.CrossRoadCoordinatesPedes[group];
                        if (_grid.Cars.Any(x=> x.IsInPosition(cr)) || _grid.Pedestrians.Any(x => x.IsInPosition(pd)))
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
        }
    }
}
