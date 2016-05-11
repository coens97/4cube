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
        };

        public Lane[] LanesP { get; } =new []
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(90,36,290,68), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(110,332,260,364), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}}
        };
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

        public void CalculateLane(Lane l)
        {
            switch (l.DirectionLane)
            {
                case Direction.Up:
                    l.EnterPoint = new Tuple<int, int>((l.BoundingBox.Item3 - l.BoundingBox.Item1)/2, l.BoundingBox.Item4);
                    l.ExitPoint = new Tuple<int, int>((l.BoundingBox.Item3 - l.BoundingBox.Item1)/2, l.BoundingBox.Item2);
                    break;
                case Direction.Down:
                    l.EnterPoint = new Tuple<int, int>((l.BoundingBox.Item3 - l.BoundingBox.Item1) / 2, l.BoundingBox.Item2);
                    l.ExitPoint = new Tuple<int, int>((l.BoundingBox.Item3 - l.BoundingBox.Item1) / 2, l.BoundingBox.Item4);
                    break;
                case Direction.Left:
                    l.EnterPoint = new Tuple<int, int>(l.BoundingBox.Item3,(l.BoundingBox.Item4- l.BoundingBox.Item2)/2);
                    l.ExitPoint = new Tuple<int, int>(l.BoundingBox.Item3,(l.BoundingBox.Item4- l.BoundingBox.Item2)/2);
                    break;
                case Direction.Right:
                    l.EnterPoint = new Tuple<int, int>(l.BoundingBox.Item1, (l.BoundingBox.Item4 - l.BoundingBox.Item2) / 2);
                    l.ExitPoint = new Tuple<int, int>(l.BoundingBox.Item1, (l.BoundingBox.Item4 - l.BoundingBox.Item2) / 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Config()
        {
            CrossRoadCoordinatesCars[TrafficLightGroup.A1] = new[]
            {
                LanesA[0].BoundingBox, LanesA[9].BoundingBox, LanesA[10].BoundingBox
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A2] = new[]
            {
                LanesA[6].BoundingBox, LanesA[7].BoundingBox, LanesA[8].BoundingBox
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A3] = new[] //3,4,5
            {
                LanesA[3].BoundingBox, LanesA[4].BoundingBox, LanesA[6].BoundingBox
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A4] = new[] //1,2,3
            {
                LanesA[0].BoundingBox, LanesA[1].BoundingBox, LanesA[3].BoundingBox
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B1] = new[] //2,5
            {
                LanesB[2].BoundingBox, LanesB[7].BoundingBox
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B2] = new[] //6 for cars 
            {
                LanesB[9].BoundingBox,
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B3] = new[] //3 
            {
                LanesB[3].BoundingBox
            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B4] = new[]
            {
                LanesB[6].BoundingBox
            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B5] = new[]
            {
                LanesB[0].BoundingBox,
            };

            CrossRoadCoordinatesPedes[TrafficLightGroup.B3] = new[]
            {
                LanesP[0].BoundingBox
            };
            CrossRoadCoordinatesPedes[TrafficLightGroup.B2] = new[]
            {
                LanesP[1].BoundingBox
            };
            PedstrainSpawn[TrafficLightGroup.B3] = new[]
            {
                new Tuple<int, int, Direction>(90, 52, Direction.Left)
            };
            PedstrainSpawn[TrafficLightGroup.B2] = new[]
            {
                new Tuple<int, int, Direction>(110, 348, Direction.Right),
            };

            _crossRoadALanes = LanesA.Select(x => x.BoundingBox).ToArray();

            _crossRoadBLanes = LanesB.Select(x => x.BoundingBox).ToArray();

            foreach (var lane in LanesA)
            {
                CalculateLane(lane);
            }

            foreach (var lane in LanesB)
            {
                CalculateLane(lane);
            }
        }
    }
}