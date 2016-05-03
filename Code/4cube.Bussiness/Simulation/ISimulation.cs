using _4cube.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Bussiness.Simulation
{
    public interface ISimulation
    {
        void ChangeSpeed(double n);
        void Pause();
        void Start(GridEntity g);
        void Stop();


    }
}
