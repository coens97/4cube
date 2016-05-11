using System;
using System.Linq;
using _4cube.Common;
using _4cube.Common.Ai;

namespace _4cube.Bussiness.Simulation
{
    public static class SimulationUtility
    {
        public static bool IsInPosition(this CarEntity car, Tuple<int, int, int, int>[] positions, int gridx, int gridy)
        {
            return IsInPosition(car.X, car.Y, positions, gridx, gridy);
        }

        public static bool IsInPosition(this PedestrianEntity ped, Tuple<int, int, int, int>[] positions, int gridx, int gridy)
        {
            return IsInPosition(ped.X, ped.Y, positions, gridx, gridy);
        }

        public static bool IsInPosition(int x, int y, Tuple<int, int, int, int>[] positions, int gridx, int gridy)
        {
            return positions.Any(pos => x.InBetween(pos.Item1 + gridx, pos.Item3 + gridx) && y.InBetween(pos.Item2 + gridy, pos.Item4 + gridy));
        }

        public static Tuple<int, int> GetGridPosition(int x, int y, int gridWidth, int gridHeight)
        {
            return new Tuple<int, int>(
                (int)Math.Floor(x / Convert.ToDecimal(gridWidth)) * gridWidth,
                (int)Math.Floor(y / Convert.ToDecimal(gridHeight)) * gridHeight);
        }

        public static bool InBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }

        public static bool IsInPosition(int x, int y, Tuple<int, int, int, int> pos)
        {
            return x.InBetween(pos.Item1, pos.Item3) && y.InBetween(pos.Item2, pos.Item4);
        }

        public static Tuple<int, int> Rotate(int x, int y, int originX, int originY, Direction d)
        {
            double angleInRadians = 0;
            double cosTheta;
            double sinTheta;

            switch (d)
            {
                case Direction.Right:
                    angleInRadians = 90 * (Math.PI / 180);
                    break;
                case Direction.Down:
                    angleInRadians = 180 * (Math.PI / 180);
                    break;
                case Direction.Left:
                    angleInRadians = 270 * (Math.PI / 180);
                    break;
                case Direction.Up:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }

            cosTheta = Math.Cos(angleInRadians);
            sinTheta = Math.Sin(angleInRadians);
            return new Tuple<int, int>((int) (cosTheta*(x - originX) - sinTheta*(y - originY) + originX), (int) (sinTheta*(x - originX) + cosTheta*(y - originY) + originY));
        }
    }
}