using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Bussiness
{
    interface ISimulation
    {
        void ChangeSpeed(int n);
        void Pause();
        void Start();
        void Stop();


    }
}
