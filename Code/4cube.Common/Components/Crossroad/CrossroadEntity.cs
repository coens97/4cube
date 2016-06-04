using System.ComponentModel;
using PropertyChanged;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    [ImplementPropertyChanged]
    public abstract class CrossroadEntity: ComponentEntity , INotifyPropertyChanged
    {
        [DoNotNotify]
        public abstract TrafficLightGroup[] GreenLightTimeEntities { get; }

        public int CurrentGreenLightGroup { get; set; } = 0;

        [DoNotNotify]
        public int LastTimeSwitched { get; set; } = 0;

        public bool LightOrange { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
