using System;
using System.Collections.Generic;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public interface IConfig
    {
        int GridWidth { get; }
        int GridHeight { get; }
        int CarDistance { get; }
        Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; }
        Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; }

        Tuple<int, int, int, int>[] GetAllLanesOfTrafficLight(Type t);
    }
}