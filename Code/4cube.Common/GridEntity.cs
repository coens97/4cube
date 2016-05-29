using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using _4cube.Common.Ai;
using _4cube.Common.Components;

namespace _4cube.Common
{
    [ImplementPropertyChanged]
    public class GridEntity : INotifyPropertyChanged
    {
        public ObservableCollection<CarEntity> Cars { get; set; }
        public ObservableCollection<ComponentEntity> Components { get; set; }
        public double Height { get; set; }
        public ObservableCollection<PedestrianEntity> Pedestrians { get; set; }
        public double Width { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
