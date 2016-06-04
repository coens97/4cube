using System.ComponentModel;
using PropertyChanged;

namespace _4cube.Common
{
    [ImplementPropertyChanged]
    public class GridContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public GridEntity Grid { get; set; } = new GridEntity();
    }
}
