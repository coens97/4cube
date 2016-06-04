using System;
using System.Collections.Generic;
using _4cube.Common;
using System.Linq;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public class Config : IConfig
    {
        public int GridWidth { get; } = 400;
        public int GridHeight { get; } = 400;
        public int CarDistance { get; } = 20;
        public int CarSpeed { get; } = 4;
        public int PedestrianSpeed { get; } = 1;
        public int GetScale { get; } = 2;


        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesCars { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();
        public Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]> CrossRoadCoordinatesPedes { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, int, int>[]>();

        public Dictionary<TrafficLightGroup, Tuple<int, int, Direction>> PedstrainSpawn { get; } = new Dictionary<TrafficLightGroup, Tuple<int, int, Direction>>();

        private readonly Tuple<int, int, int, int>[] _crossRoadASensors;
        private readonly Tuple<int, int, int, int>[] _crossRoadBSensors;

        public Lane[] LanesA { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 172, 122), DirectionLane = Direction.Down, OutgoingDiretion = new []{ Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(172, 0, 222, 122), DirectionLane = Direction.Down, OutgoingDiretion = new []{ Direction.Down,Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(222, 0, 278, 122), DirectionLane = Direction.Up, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 122, 399, 172), DirectionLane = Direction.Left, OutgoingDiretion = new []{ Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 172, 399, 222), DirectionLane = Direction.Left, OutgoingDiretion = new []{ Direction.Left,Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 222, 399, 278), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(228, 278, 278, 399), DirectionLane = Direction.Up, OutgoingDiretion = new []{ Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(178, 278, 228, 399), DirectionLane = Direction.Up, OutgoingDiretion = new []{ Direction.Up,Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 178, 399), DirectionLane = Direction.Down, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 228, 122, 278), DirectionLane = Direction.Right, OutgoingDiretion = new []{ Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 178, 122, 228), DirectionLane = Direction.Right, OutgoingDiretion = new []{ Direction.Right,Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 122, 122, 178), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}}
        };
        public Lane[] LanesB { get; }= 
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 202, 122), DirectionLane = Direction.Down, OutgoingDiretion = new [] {Direction.Down, Direction.Left, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(202, 0, 278, 122), DirectionLane = Direction.Up, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 122, 399, 172), DirectionLane = Direction.Left, OutgoingDiretion = new [] {Direction.Up, Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 172, 399, 222), DirectionLane = Direction.Left, OutgoingDiretion = new [] {Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(278, 222, 399, 278), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(202, 278, 278, 399), DirectionLane = Direction.Up, OutgoingDiretion = new [] {Direction.Up, Direction.Left, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 202, 399), DirectionLane = Direction.Down, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 228, 122, 278), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Down, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 178, 122, 228), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 0, 202, 122), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}}
        };

        public Lane[] StraightRoad { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 122, 100, 200), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(300, 122, 399, 200), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 200, 100, 378), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(300, 200, 399, 378), DirectionLane = Direction.Left, OutgoingDiretion = new [] {Direction.Left}},
        };
        public Lane[] CurvedRoad { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 122, 100, 200), DirectionLane = Direction.Right, OutgoingDiretion = new [] {Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(300, 122, 399, 399), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 200, 399), DirectionLane = Direction.Down, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(200, 278, 278, 399), DirectionLane = Direction.Up, OutgoingDiretion = new [] {Direction.Left}},
        };

        public Lane[] GetLanesOfComponent(ComponentEntity component)
        {
            if (component is CrossroadAEntity)
                return LanesA;
            if (component is CrossroadBEntity)
                return LanesB;
            if (component is StraightRoadEntity)
                return StraightRoad;
            if (component is CurvedRoadEntity)
                return CurvedRoad;
            return null;
        }

        public Lane[] LanesP { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(90,36,290,68), DirectionLane = Direction.Left, OutgoingDiretion = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(110,332,260,364), DirectionLane = Direction.Right, OutgoingDiretion = new Direction[] {}}
        };
        public Tuple<int, int, int, int>[] GetTrafficLightSensors(Type t)
        {
            if (t == typeof(CrossroadAEntity))
            {
                return _crossRoadASensors;
            }
            else if (t == typeof(CrossroadBEntity))
            {
                return _crossRoadBSensors;
            }
            return null;
        }

        public void CalculateLane(Lane l)
        {
            switch (l.DirectionLane)
            {
                case Direction.Up:
                    l.EnterPoint = new Tuple<int, int>((l.BoundingBox.Item1 + l.BoundingBox.Item3)/2, l.BoundingBox.Item4);
                    l.ExitPoint = new Tuple<int, int>((l.BoundingBox.Item1 + l.BoundingBox.Item3)/2, l.BoundingBox.Item2);
                    break;
                case Direction.Down:
                    l.EnterPoint = new Tuple<int, int>((l.BoundingBox.Item1 + l.BoundingBox.Item3) / 2, l.BoundingBox.Item2);
                    l.ExitPoint = new Tuple<int, int>((l.BoundingBox.Item1 + l.BoundingBox.Item3) / 2, l.BoundingBox.Item4);
                    break;
                case Direction.Left:
                    l.EnterPoint = new Tuple<int, int>(l.BoundingBox.Item3,(l.BoundingBox.Item4+ l.BoundingBox.Item2)/2);
                    l.ExitPoint = new Tuple<int, int>(l.BoundingBox.Item1,(l.BoundingBox.Item4+ l.BoundingBox.Item2)/2);
                    break;
                case Direction.Right:
                    l.EnterPoint = new Tuple<int, int>(l.BoundingBox.Item1, (l.BoundingBox.Item4 + l.BoundingBox.Item2) / 2);
                    l.ExitPoint = new Tuple<int, int>(l.BoundingBox.Item3, (l.BoundingBox.Item4 + l.BoundingBox.Item2) / 2);
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

            PedstrainSpawn[TrafficLightGroup.B3] = new Tuple<int, int, Direction>(90, 52, Direction.Right);
            PedstrainSpawn[TrafficLightGroup.B2] = new Tuple<int, int, Direction>(310, 350, Direction.Left);

            _crossRoadASensors = LanesA.Where(x => x.OutgoingDiretion.Any())
                .Select(GetSensorBounds).ToArray();

            _crossRoadBSensors = LanesB
                .Where(x => x.OutgoingDiretion.Any())
                .Select(x => x.BoundingBox).ToArray();

            foreach (var lane in LanesA)
            {
                CalculateLane(lane);
            }

            foreach (var lane in LanesB)
            {
                CalculateLane(lane);
            }
        }

        private Tuple<int, int, int, int> GetSensorBounds(Lane l)
        {
            var d = (int)(CarDistance*1.5);
            var b = l.BoundingBox;
            switch (l.DirectionLane)
            {
                case Direction.Up:
                    return new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item3, b.Item2 + d);
                case Direction.Right:
                    return new Tuple<int, int, int, int>(b.Item3 - d, b.Item2, b.Item3, b.Item4);
                case Direction.Down:
                    return new Tuple<int, int, int, int>(b.Item1, b.Item4 - d, b.Item3, b.Item4);
                case Direction.Left:
                    return new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item1 + d, b.Item4);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}