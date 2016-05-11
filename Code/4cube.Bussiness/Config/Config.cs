using System;
using System.Collections.Generic;
using _4cube.Common;
using System.Linq;
using _4cube.Common.Components.Crossroad;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public class Config : IConfig
    {
        public int GridWidth { get; } = 400;
        public int GridHeight { get; } = 400;
        public int CarDistance { get; } = 10;
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();

        public Dictionary<TrafficLightGroup, Tuple<int, int, Direction>[]> PedstrainSpawn { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, Direction>[]>();

        private readonly Tuple<int, int, int, int>[] _crossRoadALanes;
        private readonly Tuple<int, int, int, int>[] _crossRoadBLanes;

        public Lane[] LanesA { get; } = new[]
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 172, 122), DirectionLane = Direction.Down, OutgoingDiretion = new []{ Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(172, 0, 222, 122), DirectionLane = Direction.Down, OutgoingDiretion = new []{ Direction.Down,Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(222, 0, 278, 122), DirectionLane = Direction.Up, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 122, 400, 172), DirectionLane = Direction.Left, OutgoingDiretion = new []{ Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 172, 400, 222), DirectionLane = Direction.Left, OutgoingDiretion = new []{ Direction.Left,Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 222, 400, 278), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(228, 278, 278, 400), DirectionLane = Direction.Up, OutgoingDiretion = new []{ Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(178, 278, 228, 400), DirectionLane = Direction.Up, OutgoingDiretion = new []{ Direction.Up,Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 178, 400), DirectionLane = Direction.Down, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 228, 0, 278), DirectionLane = Direction.Right, OutgoingDiretion = new []{ Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 178, 0, 228), DirectionLane = Direction.Right, OutgoingDiretion = new []{ Direction.Right,Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 122, 0, 178), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}}
        };
        public Lane[] LanesB { get; }= new[]
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 202, 122), DirectionLane = Direction.Down, OutgoingDiretion = new [] {Direction.Down, Direction.Left, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(202, 0, 278, 122), DirectionLane = Direction.Up, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 122, 400, 172), DirectionLane = Direction.Left, OutgoingDiretion = new [] {Direction.Up, Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 172, 400, 222), DirectionLane = Direction.Left, OutgoingDiretion = new [] {Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 222, 400, 278), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(202, 278, 278, 400), DirectionLane = Direction.Up, OutgoingDiretion = new [] {Direction.Up, Direction.Left, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 202, 400), DirectionLane = Direction.Down, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 228, 122, 278), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Down, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 178, 122, 228), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 202, 122), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}}
        }
        public Tuple<int, int, int, int>[] GetAllLanesOfTrafficLight(Type t)
        {
            if (t == typeof(CrossroadAEntity))
            {
                return _crossRoadALanes;
            }
            else if (t == typeof(CrossroadBEntity))
            {
                return _crossRoadBLanes;
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

            _crossRoadALanes = CrossRoadCoordinatesCars[TrafficLightGroup.A1].Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A2])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A3])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.A4])
                    as Tuple<int, int, int, int>[];

            _crossRoadBLanes =
                CrossRoadCoordinatesCars[TrafficLightGroup.B1].Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B2])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B3])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B4])
                    .Concat(CrossRoadCoordinatesCars[TrafficLightGroup.B5])
                    as Tuple<int, int, int, int>[];
        }
    }
}