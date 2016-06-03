using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;
using System.IO;
using _4cube.Common.Components.Crossroad;
using _4cube.Data;

namespace _4cube.Bussiness
{
    public class GridModel : IGridModel
    {
        public GridEntity Grid { get; set; } = new GridEntity
        {
            Pedestrians = new ObservableCollection<Common.Ai.PedestrianEntity>(),
            Components = new ObservableCollection<ComponentEntity>(),
            Cars = new ObservableCollection<Common.Ai.CarEntity>()
        };

        private readonly IGridData _datalayer;
        public GridModel(IGridData datalayer)
        {
            _datalayer = datalayer;
        }

        public void AddComponent(ComponentEntity component)
        {
            Grid.Components.Add(component);
        }

        public void DeleteComponent(ComponentEntity component)
        {
            Grid.Components.Remove(component);
        }

        public void OpenFile(string path)
        {
            Grid = _datalayer.OpenFile(path);
        }

        public void ResizeGrid(int w, int h)
        {
            if (Grid.Components.Any(x => x.X > w || x.Y > h))
            {
                throw new Exception("Components are outside of the grid");
            }
            Grid.Width = w;
            Grid.Height = h;
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
            _datalayer.SaveFile(path, Grid);
        }
    }
}
