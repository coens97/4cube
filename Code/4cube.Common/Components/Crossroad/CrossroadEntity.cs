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
<<<<<<< HEAD
        public List<TrafficLightGroup> GreenLightTimeEntities { get; set; }
=======
        public GreenLightTimeEntity[] GreenLightTimeEntities { get; set; }
>>>>>>> origin/master

        public int CurrentGreenLightGroup { get; set; } = 0;

        [DoNotNotify]
        public double LastTimeSwitched { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
