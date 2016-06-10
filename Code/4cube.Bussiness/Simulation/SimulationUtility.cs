using System;
using System.Linq;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;

namespace _4cube.Bussiness.Simulation
{
    public static class SimulationUtility
    {
        public static bool IsInPosition(this CarEntity car, Tuple<int, int, int, int>[] positions, int gridx, int gridy, int gridWidth, int gridHeight, Direction d)
        {
            return positions.IsInPosition(car.X, car.Y, gridx, gridy, gridWidth,gridHeight,d);
        }

        public static bool IsInPosition(this PedestrianEntity ped, Tuple<int, int, int, int>[] positions, int gridx, int gridy, int gridWidth, int gridHeight, Direction d)
        {
            return positions.IsInPosition(ped.X, ped.Y, gridx, gridy, gridWidth, gridHeight, d);
        }

        /*public static bool IsInPosition(int x, int y, Tuple<int, int, int, int>[] positions, int gridx, int gridy)
        {
            return positions.Any(pos => pos.IsInPosition(x, y, gridx, gridy));
        }*/

        public static Tuple<int, int> GetGridPosition(int x, int y, int gridWidth, int gridHeight)
        {
            return new Tuple<int, int>(
                (int)Math.Floor(x / Convert.ToDecimal(gridWidth)) * gridWidth,
                (int)Math.Floor(y / Convert.ToDecimal(gridHeight)) * gridHeight);
        }

        public static bool InBetween(this int value, int minimum, int maximum)
        {
            if (minimum > maximum)
            {
                var t = minimum;
                minimum = maximum;
                maximum = t;
            }
            return value >= minimum && value <= maximum;
        }

        public static bool IsInPosition(this Tuple<int, int, int, int> pos, int x, int y)
        {
            return x.InBetween(pos.Item1, pos.Item3) && y.InBetween(pos.Item2, pos.Item4);
        }

        public static bool IsInPosition(this Tuple<int, int, int, int>[] inp, int x, int y, int gridx, int gridy, int gridWidth, int gridHeight, Direction d)
        {
            return inp.Any(tup => tup.IsInPosition(x, y, gridx, gridy, gridWidth, gridHeight, d));
        }

        public static bool IsInPosition(this Tuple<int, int, int, int> inp, int x, int y, int gridx, int gridy, int gridWidth, int gridHeight, Direction d)
        {
            var pos = inp.Rotate(gridWidth, gridHeight,d);
            return x.InBetween(pos.Item1 + gridx, pos.Item3 + gridx) && y.InBetween(pos.Item2 + gridy, pos.Item4 + gridy);
        }

        public static Tuple<int, int> Rotate(int x, int y, int originX, int originY, Direction d)
        {
            x -= originX;
            y -= originY;

            var oldX = x;
            var oldY = y;
            switch (d)
            {
                case Direction.Right:
                    x = -y -1;
                    y = oldX;
                    break;
                case Direction.Down:
                    x = -x - 1;
                    y = -oldY - 1;
                    break;
                case Direction.Left:
                    x = y;
                    y = -oldX - 1;
                    break;
                case Direction.Up:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }

            
            return new Tuple<int, int>(x + originX, y + originY);
        }

        public static Tuple<int, int, int, int>[] Rotate(this Tuple<int, int, int, int>[] t, int gridX, int gridY,
            int gridWidth, int gridHeight,Direction d)
        {
            var result = new Tuple<int, int, int, int>[t.Length];

            for (var i = 0; i < t.Length; i++)
            {
                result[i] = t[i].Rotate(gridWidth, gridHeight, d);
            }

            return result;
        }

        public static Tuple<int, int> Rotate(this Tuple<int, int> t, Direction d, int gridWidth, int gridHeight)
        {
            var originX = gridWidth / 2;
            var originY = gridHeight / 2;

            return Rotate(t.Item1, t.Item2, originX, originY, d);
        }

        public static Tuple<int, int, int, int> Rotate(this Tuple<int, int, int, int> t, int gridWidth, int gridHeight, Direction d)
        {
            var originX = gridWidth / 2;
            var originY = gridHeight / 2;

            var r1 = Rotate(t.Item1, t.Item2, originX, originY, d);
            var r2 = Rotate(t.Item3, t.Item4, originX, originY, d);
            return new Tuple<int, int, int, int>(r1.Item1, r1.Item2, r2.Item1, r2.Item2);
        }

        public static Direction RotatedDirection(this Direction old, Direction changed)
        {
            return (Direction)(((int)old + (int)changed) % 4);
        }

        public static Direction RotatedDirectionInv(this Direction old, Direction changed)
        {
            return (Direction)((3 - (int)old + (int)changed) % 4);
        }

    }
}