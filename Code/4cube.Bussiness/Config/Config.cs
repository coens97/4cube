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
        public int CarSpeed { get; } = 10;
        public int PedestrianSpeed { get; } = 1;
        public int GetScale { get; } = 2;


        public Dictionary<TrafficLightGroup, Lane[]> CrossRoadCoordinatesCars { get; } = new Dictionary<TrafficLightGroup, Lane[]>();
        public Dictionary<TrafficLightGroup, Lane[]> CrossRoadCoordinatesPedes { get; } = new Dictionary<TrafficLightGroup, Lane[]>();

        public Lane[] LanesA { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(121, 0, 171, 121), DirectionLane = Direction.Down, OutgoingDirection = new []{ Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(171, 0, 221, 121), DirectionLane = Direction.Down, OutgoingDirection = new []{ Direction.Down,Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(221, 0, 277, 121), DirectionLane = Direction.Up, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 121, 399, 171), DirectionLane = Direction.Left, OutgoingDirection = new []{ Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 171, 399, 221), DirectionLane = Direction.Left, OutgoingDirection = new []{ Direction.Left,Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 221, 399, 277), DirectionLane = Direction.Right, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(227, 277, 277, 399), DirectionLane = Direction.Up, OutgoingDirection = new []{ Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(177, 277, 227, 399), DirectionLane = Direction.Up, OutgoingDirection = new []{ Direction.Up,Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(121, 277, 177, 399), DirectionLane = Direction.Down, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 227, 121, 277), DirectionLane = Direction.Right, OutgoingDirection = new []{ Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 177, 121, 227), DirectionLane = Direction.Right, OutgoingDirection = new []{ Direction.Right,Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 121, 121, 177), DirectionLane = Direction.Left, OutgoingDirection = new Direction[] {}}
        };
        public Lane[] LanesB { get; }= 
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(121, 1, 161, 121), DirectionLane = Direction.Down, OutgoingDirection = new [] { Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(161, 1, 201, 121), DirectionLane = Direction.Down, OutgoingDirection = new []{ Direction.Right, Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(201, 1, 277, 121), DirectionLane = Direction.Up, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 121, 399, 171), DirectionLane = Direction.Left, OutgoingDirection = new [] {Direction.Up, Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 171, 399, 221), DirectionLane = Direction.Left, OutgoingDirection = new [] {Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(277, 221, 399, 277), DirectionLane = Direction.Right, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(239, 277, 277, 399), DirectionLane = Direction.Up, OutgoingDirection = new [] {Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(201, 277, 239, 399), DirectionLane = Direction.Up, OutgoingDirection = new [] {Direction.Left, Direction.Up }},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(121, 277, 201, 399), DirectionLane = Direction.Down, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(1, 227, 121, 277), DirectionLane = Direction.Right, OutgoingDirection = new [] {Direction.Down, Direction.Right}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(1, 177, 121, 227), DirectionLane = Direction.Right, OutgoingDirection = new [] {Direction.Up}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(1, 121, 121, 177), DirectionLane = Direction.Left, OutgoingDirection = new Direction[] {}},
        };

        public Tuple<int, int, int, int> TrafficCenter { get; } = new Tuple<int, int, int, int>(122, 122, 278, 278);

        public Lane[] StraightRoad { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 122, 100, 200), DirectionLane = Direction.Left, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(300, 122, 399, 200), DirectionLane = Direction.Left, OutgoingDirection = new [] {Direction.Left}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(0, 200, 100, 278), DirectionLane = Direction.Right, OutgoingDirection = new [] {Direction.Right }},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(300, 200, 399,278), DirectionLane = Direction.Right, OutgoingDirection = new Direction[] {}},
        };
        public Lane[] CurvedRoad { get; } =
        {
            new Lane { BoundingBox = new Tuple<int, int, int, int>(1, 122, 200, 200), DirectionLane = Direction.Left, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(1, 200, 122, 278), DirectionLane = Direction.Right, OutgoingDirection = new [] {Direction.Down}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(122, 278, 200, 399), DirectionLane = Direction.Down, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(200, 200, 278, 399), DirectionLane = Direction.Up, OutgoingDirection = new [] {Direction.Left}},
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
            new Lane { BoundingBox = new Tuple<int, int, int, int>(90,36,290,68), DirectionLane = Direction.Left, OutgoingDirection = new Direction[] {}},
            new Lane { BoundingBox = new Tuple<int, int, int, int>(110,332,260,364), DirectionLane = Direction.Right, OutgoingDirection = new Direction[] {}}
        };

        public Dictionary<Lane, Lane> CrossCarLanePedesLane { get; } = new Dictionary<Lane, Lane>();

        public void CalculateLane(Lane l)
        {
            var d = (int)(CarDistance * 1.5);
            var b = l.BoundingBox;
            switch (l.DirectionLane)
            {
                case Direction.Up:
                    l.EnterPoint = new Tuple<int, int>((b.Item1 + b.Item3)/2, b.Item4);
                    l.ExitPoint = new Tuple<int, int>((b.Item1 + b.Item3)/2, b.Item2);
                    l.EnterBounding = new Tuple<int, int, int, int>(b.Item1, b.Item4 - d, b.Item3, b.Item4);
                    l.ExitBounding = new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item3, b.Item2 + d);
                    break;
                case Direction.Down:
                    l.EnterPoint = new Tuple<int, int>((b.Item1 + b.Item3) / 2, b.Item2);
                    l.ExitPoint = new Tuple<int, int>((b.Item1 + b.Item3) / 2, b.Item4);
                    l.EnterBounding = new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item3, b.Item2 + d);
                    l.ExitBounding = new Tuple<int, int, int, int>(b.Item1, b.Item4 - d, b.Item3, b.Item4);
                    break;
                case Direction.Left:
                    l.EnterPoint = new Tuple<int, int>(b.Item3,(b.Item4+ b.Item2)/2);
                    l.ExitPoint = new Tuple<int, int>(b.Item1,(b.Item4+ b.Item2)/2);
                    l.EnterBounding = new Tuple<int, int, int, int>(b.Item3 - d, b.Item2, b.Item3, b.Item4);
                    l.ExitBounding = new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item1 + d, b.Item4);
                    break;
                case Direction.Right:
                    l.EnterPoint = new Tuple<int, int>(b.Item1, (b.Item4 + b.Item2) / 2);
                    l.ExitPoint = new Tuple<int, int>(b.Item3, (b.Item4 + b.Item2) / 2);
                    l.EnterBounding = new Tuple<int, int, int, int>(b.Item1, b.Item2, b.Item1 + d, b.Item4);
                    l.ExitBounding = new Tuple<int, int, int, int>(b.Item3 - d, b.Item2, b.Item3, b.Item4);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Config()
        {
            CrossCarLanePedesLane[LanesB[0]] = LanesP[0];
            CrossCarLanePedesLane[LanesB[1]] = LanesP[0];
            CrossCarLanePedesLane[LanesB[2]] = LanesP[0];
            CrossCarLanePedesLane[LanesB[7]] = LanesP[1];
            CrossCarLanePedesLane[LanesB[8]] = LanesP[1];
            CrossCarLanePedesLane[LanesB[9]] = LanesP[1];

            CrossRoadCoordinatesCars[TrafficLightGroup.A1] = new[]
            {
                LanesA[0],LanesA[9], LanesA[10]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A2] = new[]
            {
                LanesA[6], LanesA[7], LanesA[8]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A3] = new[] //3,4,5
            {
                LanesA[3], LanesA[4], LanesA[6]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.A4] = new[] //1,2,3
            {
                LanesA[0], LanesA[1], LanesA[3]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B1] = new[] //2,5
            {
                LanesB[3], LanesB[9]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B2] = new[] //6 for cars 
            {
                LanesB[10]
            };

            CrossRoadCoordinatesCars[TrafficLightGroup.B3] = new[] //3 
            {
                LanesB[4]
            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B4] = new[]
            {
                LanesB[6], LanesB[7],
            };
            CrossRoadCoordinatesCars[TrafficLightGroup.B5] = new[]
            {
                LanesB[0], LanesB[1],
            };

            CrossRoadCoordinatesPedes[TrafficLightGroup.B3] = new[]
            {
                LanesP[0]
            };
            CrossRoadCoordinatesPedes[TrafficLightGroup.B2] = new[]
            {
                LanesP[1]
            };

            LanesA.AsParallel().ForAll(CalculateLane);
            LanesB.AsParallel().ForAll(CalculateLane);
            CurvedRoad.AsParallel().ForAll(CalculateLane);
            StraightRoad.AsParallel().ForAll(CalculateLane);
            LanesP.AsParallel().ForAll(CalculateLane);
        }
    }
}