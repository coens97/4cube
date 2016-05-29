
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace _4cube.Common.Ai
{
    [ImplementPropertyChanged]
    public class CarEntity : INotifyPropertyChanged, IPosition
    {
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}