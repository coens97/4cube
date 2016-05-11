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
            double radians = 0;
            double cosTheta;
            double sinTheta;

            switch (d)
            {
                case Direction.Right:
                    radians = 0.5 * Math.PI;
                    break;
                case Direction.Down:
                    radians = Math.PI;
                    break;
                case Direction.Left:
                    radians = 1.5 * Math.PI;
                    break;
                case Direction.Up:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }

            cosTheta = Math.Cos(radians);
            sinTheta = Math.Sin(radians);
            return new Tuple<int, int>((int) (cosTheta*(x - originX) - sinTheta*(y - originY) + originX), (int) (sinTheta*(x - originX) + cosTheta*(y - originY) + originY));
        }

        public static Tuple<int, int, int, int>[] Rotate(Tuple<int, int, int, int>[] t, int gridX, int gridY,
            int gridWidth, int gridHeight,Direction d)
        {
            var originX = gridWidth/2+gridX;
            var originY = gridHeight/2+gridY;

            var result = new Tuple<int, int, int, int>[t.Length];

            for (var i = 0; i < t.Length; i++)
            {
                var r1 = Rotate(t[i].Item1, t[i].Item2, originX, originY, d);
                var r2 = Rotate(t[i].Item3, t[i].Item4, originX, originY, d);
                result[i]= new Tuple<int, int, int, int>(r1.Item1,r1.Item2,r2.Item1,r2.Item2);
            }

            return result;
        }

    }
}