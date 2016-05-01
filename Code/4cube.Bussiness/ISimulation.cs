using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Bussiness
{
    public interface ISimulation
    {
        void ChangeSpeed(double n);
        void Pause();
        void Start();
        void Stop();


    }
}
