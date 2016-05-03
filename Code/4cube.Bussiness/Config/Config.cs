using System;
using System.Collections.Generic;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public class Config : IConfig
    {
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();

        public Config()
        {
            CrossRoadCoordinatesCars[TrafficLightGroup.A1] = new[]
            {
                new Tuple<int, int, int, int>(122, 0, 172, 122),
                new Tuple<int, int, int, int>(122, 278, 0, 227),
                new Tuple<int, int, int, int>(122, 227, 0, 178)
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A2] = new[]
            {
                new Tuple<int, int, int, int>(227, 278, 278, 400),
                new Tuple<int, int, int, int>(178, 278, 227, 400),
                new Tuple<int, int, int, int>(122, 278, 0, 227)

            };
        }
    }
}