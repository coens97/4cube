using System;
using _4cube.Common;

namespace _4cube.Bussiness.Config
{
    public class Lane
    {
        public Tuple<int, int, int, int> BoundingBox { get; set; }
        public Direction DirectionLane { get; set; }
        public Direction[] OutgoingDiretion { get; set; }
        public Tuple<int, int> EnterPoint { get; set; }
        public Tuple<int, int> ExitPoint { get; set; }
    }
}