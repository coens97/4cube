using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Data;
using System.Timers;

namespace _4cube.Bussiness.Simulation

{
    public class Simulation : ISimulation
    {
        private GridEntity _grid;
        // private IReport _report;
        private double _time = 0;
        private Timer _timer;

        public Simulation()
        {
            
        }

        public void ChangeSpeed(double n)
        {
            _timer.Interval = n;

        }

        public void Pause(GridEntity g)
        {
            _timer.Stop();
            _time = _timer.Interval;
            _grid = g;
           

        }


        public void Start(GridEntity g)
        {
            _grid = g;
            _timer = new Timer(_time);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _time = 0;
        }
    }
}
