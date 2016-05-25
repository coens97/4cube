using System.Collections.Generic;
using PropertyChanged;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    [ImplementPropertyChanged]
    public class CrossroadEntity:ComponentEntity
    {
        [DoNotNotify]
        public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }

        public int CurrentGreenLightGroup { get; set; }

        [DoNotNotify]
        public double LastTimeSwitched { get; set; }
    }
}
