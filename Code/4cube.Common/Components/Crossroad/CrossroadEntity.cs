using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    [ImplementPropertyChanged]
    public class CrossroadEntity: ComponentEntity , INotifyPropertyChanged
    {
        [DoNotNotify]
        public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }

        public int CurrentGreenLightGroup { get; set; }

        [DoNotNotify]
        public double LastTimeSwitched { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
