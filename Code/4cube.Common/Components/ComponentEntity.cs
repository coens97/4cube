using PropertyChanged;
using System.ComponentModel;

namespace _4cube.Common.Components
{
    [ImplementPropertyChanged]
    public class ComponentEntity : INotifyPropertyChanged
    {
        [DoNotNotify]
        public string ComponentId { get; set; }
        public int[] NrOfIncomingCars { get; set; }
        public int[] NrOfIncomingCarsSpawned { get; set; }
        public Direction Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}