using System;
using System.Linq;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Data;

namespace _4cube.Bussiness
{
    public class GridModel : IGridModel
    {
        public GridEntity Grid => _gridContainer.Grid;

        private readonly IGridData _datalayer;
        private readonly GridContainer _gridContainer;
        private readonly IConfig _config;
        public GridModel(IGridData datalayer, GridContainer gridContainer, IConfig config)
        {
            _datalayer = datalayer;
            _gridContainer = gridContainer;
            _config = config;
        }

        public void AddComponent(ComponentEntity component)
        {
            if (Grid.Components.Any(x => x.X == component.X && x.Y == component.Y))
                throw new Exception("The position in the grid is already in use");
            Grid.Components.Add(component);
        }

        public void DeleteComponent(ComponentEntity component)
        {
            CheckComponentHasCars(component);
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

        private void CheckComponentHasCars(ComponentEntity component) // Some actions can not be performed when cars are on the crossroad
        {
            var pos = new Tuple<int, int>(component.X, component.Y);
            if (Grid.Cars.Any(x => SimulationUtility.GetGridPosition(x.X , x.Y,  _config.GridWidth, _config.GridHeight).Equals(pos)))
                throw new Exception("Action can not  be performed with cars on the component.");
        }

        private static void ShiftCircular(int offset, int[] array)
        {
            if (offset == 0 || array.Length <= 1)
                return;

            offset = offset % array.Length;

            if (offset == 0)
                return;

            if (offset < 0)
                offset = array.Length + offset;

            Array.Reverse(array, 0, array.Length);
            Array.Reverse(array, 0, offset);
            Array.Reverse(array, offset, array.Length - offset);
        }

        public void RotateComponent(ComponentEntity component)
        {
            CheckComponentHasCars(component);
            ShiftCircular(1, component.NrOfIncomingCars);
            ShiftCircular(1, component.NrOfIncomingCarsSpawned);
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
