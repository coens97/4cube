using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using PropertyChanged;
using _4cube.Bussiness.Config;
using _4cube.Presentation.Annotations;

namespace _4cube.Presentation.UserControl
{
    [ImplementPropertyChanged]
    public class TrafficLightComponent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int X { get; set; }
        public int Y { get; set; }
        public Brush Color {get; set; }
        [DoNotNotify]
        public Lane Lane { get; set; }
    }
}