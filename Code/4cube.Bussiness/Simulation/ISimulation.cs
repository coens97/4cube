using _4cube.Common;

namespace _4cube.Bussiness.Simulation
{
    public interface ISimulation
    {
        void ChangeSpeed(int n);
        void Pause();
        void Start(GridEntity g);
        void Stop();
        bool Enabled { get; }
    }
}
