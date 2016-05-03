using System;
using System.Linq;
using _4cube.Common.Ai;

namespace _4cube.Bussiness.Simulation
{
    public static class SimulationUtility
    {
        public static bool IsInPosition(this CarEntity car, Tuple<int, int, int, int>[] positions)
        {
            return IsInPosition(car.X, car.Y, positions);
        }

        public static bool IsInPosition(this PedestrianEntity ped, Tuple<int, int, int, int>[] positions)
        {
            return IsInPosition(ped.X, ped.Y, positions);
        }

        public static bool IsInPosition(int x, int y, Tuple<int, int, int, int>[] positions)
        {
            return positions.Any(pos => x.InBetween(pos.Item1, pos.Item3) && y.InBetween(pos.Item2, pos.Item4));
        }

        public static bool InBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}