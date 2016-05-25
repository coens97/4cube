using PropertyChanged;

namespace _4cube.Common.Ai
{
    [ImplementPropertyChanged]
    public class PedestrianEntity
    {
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}