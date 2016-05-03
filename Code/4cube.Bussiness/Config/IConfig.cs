using System;
using System.Collections.Generic;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public interface IConfig
    {
        Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; }
        Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; }
    }
}