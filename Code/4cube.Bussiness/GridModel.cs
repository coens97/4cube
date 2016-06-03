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
        public GridEntity Grid => _gridContainer.Grid;

        private readonly IGridData _datalayer;
        private readonly GridContainer _gridContainer;
        public GridModel(IGridData datalayer, GridContainer gridContainer)
        {
            _datalayer = datalayer;
            _gridContainer = gridContainer;
        }

        public void AddComponent(ComponentEntity component)
        {
            if (Grid.Components.Any(x => x.X == component.X && x.Y == component.Y))
                throw new Exception("The position in the grid is already in use");
            Grid.Components.Add(component);
        }

        public void DeleteComponent(ComponentEntity component)
        {
            Grid.Components.Remove(component);
        }

        public void OpenFile(string path)
        {
            _gridContainer.Grid = _datalayer.OpenFile(path);
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
            component.Rotation = (Direction) (((int) component.Rotation + 1)%4);
        }

        public void SaveFile(string path)
        {
            _datalayer.SaveFile(path, Grid);
        }

        public void New()
        {
            _gridContainer.Grid = new GridEntity();
        }
    }
}
