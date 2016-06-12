using System;
using System.Collections.Generic;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness.Config
{
    public interface IConfig
    {
        int GridWidth { get; }
        int GridHeight { get; }
        int CarDistance { get; }
        int CarSpeed { get; }
        int PedestrianSpeed { get; }
        Tuple<int, int, int, int> TrafficCenter { get; }
        int GetScale { get; }

        Dictionary<TrafficLightGroup, Lane[]> CrossRoadCoordinatesCars { get; }
        Dictionary<TrafficLightGroup, Lane[]> CrossRoadCoordinatesPedes { get; }
        Dictionary<Lane, Lane> CrossCarLanePedesLane { get; }

        Lane[] LanesA { get; }
        Lane[] LanesB { get; }
        Lane[] LanesP { get; }

        Lane[] StraightRoad { get; }
        Lane[] CurvedRoad { get; }
        Lane[] GetLanesOfComponent(ComponentEntity component);
    }
}