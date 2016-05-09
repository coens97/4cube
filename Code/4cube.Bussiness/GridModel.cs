using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;
using System.IO;
using _4cube.Data;

namespace _4cube.Bussiness
{
    public class GridModel : IGridModel
    {
        public GridEntity _grid = new GridEntity
        {
            Pedestrians = new List<Common.Ai.PedestrianEntity>(),
            Components = new List<ComponentEntity>(),
            Cars = new List<Common.Ai.CarEntity>()
        };

        private IGridData _datalayer;
        public GridModel(IGridData datalayer)
        {
            _datalayer = datalayer;
        }

        public void AddComponent(ComponentEntity component)
        {
            _grid.Components.Add(component);
        }

        public void DeleteComponent(ComponentEntity component)
        {
            _grid.Components.Remove(component);
        }

        public void GreenLight(GreenLightTimeEntity e, TrafficLightGroup t, int n)
        {
            if (e.TrafficLightGroup == t)
            {
                e.Duration = n;
            }
        }

        public void OpenFile(string path)
        {
            _grid = _datalayer.OpenFile(path);
        }

        public void ResizeGrid(int w, int h)
        {
            _grid.Width = w;
            _grid.Height = h;
        }

        public void RotateComponent(ComponentEntity component)
        {
            switch (component.Rotation)
            {
                case Direction.Up:
                    component.Rotation = Direction.Right;
                    break;
                case Direction.Right:
                    component.Rotation = Direction.Down;
                    break;
                case Direction.Down:
                    component.Rotation = Direction.Left;
                    break;
                case Direction.Left:
                    component.Rotation = Direction.Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SaveFile(string path)
        {
            _datalayer.SaveFile(path, _grid);
        }
    }
}
