using System.Collections.Generic;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    public class CrossroadEntity:ComponentEntity
    {
        public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }

        public int CurrentGreenLightGroup { get; set; }

        public double LastTimeSwitched { get; set; }
    }
}
