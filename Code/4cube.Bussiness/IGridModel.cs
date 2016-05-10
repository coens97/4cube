using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness
{
    public interface IGridModel
    {
        void AddComponent(ComponentEntity component);
        void DeleteComponent(ComponentEntity component);
        void GreenLight(CrossroadEntity e, TrafficLightGroup t, int n);
        void OpenFile(string path);
        void ResizeGrid(int w, int h);
        void RotateComponent(ComponentEntity component);
        void SaveFile(string path);
    }
}
