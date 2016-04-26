namespace _4cube.Common
{
    public class ComponentEntity
    {
        public string ComponentID { get; set; }
        public int[] NrOfIncomingCars { get; set; }
        public Direction Rotation { get; set; }
        public int X;
        public int Y;
    }
}