using System;
using System.Collections.Generic;
using _4cube.Common;
using System.Linq;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public class Config : IConfig
    {
        public int GridWidth { get; } = 400;
        public int GridHeight { get; } = 400;
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();

        public Dictionary<TrafficLightGroup, Tuple<int, int, Direction>[]> PedstrainSpawn { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, Direction>[]>();

        public Tuple<int, int, int, int>[] GetAllLanesOfTrafficLight(TrafficLightGroup t)
        {
            if (t == TrafficLightGroup.A1 || t == TrafficLightGroup.A2 || t == TrafficLightGroup.A3 ||
                t == TrafficLightGroup.A4)
            {
                return
                    CrossRoadCoordinatesCars[TrafficLightGroup.A1].Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A2])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A3])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A4]) 
                    as Tuple<int, int, int, int>[];
            }
            else if (t == TrafficLightGroup.B1 || t == TrafficLightGroup.B2 || t == TrafficLightGroup.B3 ||
                     t == TrafficLightGroup.B4 || t == TrafficLightGroup.B5)
            {
                return
                    CrossRoadCoordinatesCars[TrafficLightGroup.B1].Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B2])
                        .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B3])
                        .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B4])
                        .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B5])
                        as Tuple<int, int, int, int>[];
            }
            return null;
        }


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

            CrossRoadCoordinatesCars[TrafficLightGroup.A3] = new[]  //3,4,5
            {
                new Tuple<int, int, int, int>(278,122,400,172),
                new Tuple<int, int, int, int>(278,172,400,220),
                new Tuple<int, int, int, int>(227,278,227,400)
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A4] = new[]  //1,2,3
            {
                new Tuple<int, int, int, int>(122, 0,172,122),
                new Tuple<int, int, int, int>(172,0,220,122),
                new Tuple<int, int, int, int>(278,122,400,172)
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B1] = new[] //2,5
            {
                new Tuple<int, int, int, int>(278,122,400,172),
                new Tuple<int, int, int, int>(122,278,0,228)  
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B2] = new[] //6 for cars 
            {
                new Tuple<int, int, int, int>(122,278,0,178)
                 
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B3] = new[] //3 
              {
                new Tuple<int, int, int, int>(278,172,400,222)

            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B4] = new[]
            {
                new Tuple<int, int, int, int>(278,278,204,400) 
            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B5] = new[]
             {
                new Tuple<int, int, int, int>(122,0,204,122)
            };
            CrossRoadCoordinatesPedes[TrafficLightGroup.B3] = new[]
            {
                new Tuple<int, int, int, int>(90,36,290,68)
            };
            CrossRoadCoordinatesPedes[TrafficLightGroup.B2] = new[]
             {
                new Tuple<int, int, int, int>(110,332,260,364)
            };
            PedstrainSpawn[TrafficLightGroup.B3] = new[]
            {
                new Tuple<int, int, Direction>(90, 52, Direction.Left),//x is the start, y is the middle of the y's
            };
            PedstrainSpawn[TrafficLightGroup.B2] = new[]
            {
                new Tuple<int, int, Direction>(110,348,Direction.Right), 
            };
        }
    }
}