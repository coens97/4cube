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

        public static bool IsInPosition(int x, int y, Tuple<int, int, int, int> pos)
        {
            return x.InBetween(pos.Item1, pos.Item3) && y.InBetween(pos.Item2, pos.Item4);
        }

        public static bool InBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }

        
        public static Tuple<int, int> Rotate(int x, int y, int originX, int originY, Direction d)
        {

            double angleInRadians = 90 * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Tuple<int, int>
                (
                
                    (int)
                        (cosTheta*(x - originX) -
                         sinTheta*(y - originY) + originX),
               
                    (int)
                        (sinTheta*(x - originX) +
                         cosTheta*(y - originY) + originY)
                );

        }

        public static Tuple<int, int> GetGridPosition(int x, int y, int gridWidth, int gridHeight)
        {
            // from an input x and y get the position of the lefttop corner of the grid it is in
            // width = 400 height = 400 then (100,100)->(0,0);420,20->400,0

            return new Tuple<int, int>(
                (int)Math.Floor(x/ Convert.ToDecimal(gridWidth)) * gridWidth,
                (int)Math.Floor(y / Convert.ToDecimal(gridHeight)) * gridHeight);
        }


    }
}