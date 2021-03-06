﻿using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common.Components.Crossroad
{
    public class CrossroadAEntity:CrossroadEntity
    {
        public override TrafficLightGroup[] GreenLightTimeEntities => new [] 
        {
            TrafficLightGroup.A1,
            TrafficLightGroup.A2, TrafficLightGroup.A3, TrafficLightGroup.A4
        };
    }
}
