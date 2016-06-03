using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    public class CrossroadBEntity:CrossroadEntity
    {
        public override TrafficLightGroup[] GreenLightTimeEntities => new [] {
            TrafficLightGroup.B1, TrafficLightGroup.B2, TrafficLightGroup.B3, TrafficLightGroup.B4, TrafficLightGroup.B5
        };
    }
}
