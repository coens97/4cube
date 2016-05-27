using PropertyChanged;
using System.ComponentModel;

namespace _4cube.Common.Ai
{
    [ImplementPropertyChanged]
    public class PedestrianEntity : INotifyPropertyChanged
    {
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}