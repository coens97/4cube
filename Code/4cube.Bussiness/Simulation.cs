using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Data;
using System.Timers;

namespace _4cube.Bussiness
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
            _time = n;

        }

        public void Pause()
        {
            _timer.Stop();
            _time = _timer.Interval;
            getGrid();

        }

        public IEnumerable<GridEntity> getGrid()
        {
            yield return _grid;
        }

        public void Start()
        {
            _timer = new Timer(_time);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _grid = new GridEntity();
            _time = 0;
        }
    }
}
