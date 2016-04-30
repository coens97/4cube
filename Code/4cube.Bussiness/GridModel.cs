using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Bussiness
{
    public class GridModel : IGridModel
    {
        private GridEntity _grid;

        public void AddComponent(ComponentEntity component)
        {
            throw new NotImplementedException();
        }

        public void DeleteComponent(ComponentEntity component)
        {
            throw new NotImplementedException();
        }

        public void GreenLight(GreenLightTimeEntity e, TrafficLightGroup t, int n)
        {
            throw new NotImplementedException();
        }

        public void OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public void ResizeGrid(int w, int h)
        {
            throw new NotImplementedException();
        }

        public void RotateComponent(ComponentEntity component)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
