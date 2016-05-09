namespace _4cube.Common.Components
{
    public class ComponentEntity
    {
        public string ComponentId { get; set; }
        public int[] NrOfIncomingCars { get; set; }
        public Direction Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}