using PropertyChanged;
using System.ComponentModel;

namespace _4cube.Common.Components
{
    [ImplementPropertyChanged]
    public class ComponentEntity : INotifyPropertyChanged
    {
        public int[] NrOfIncomingCars { get; set; } = {0, 0, 0, 0};
        public int[] NrOfIncomingCarsSpawned { get; set; } = { 0, 0, 0, 0 };
        public Direction Rotation { get; set; } = Direction.Up;
        public int X { get; set; }
        public int Y { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}