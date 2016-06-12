using System;
using System.Linq;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
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
            CheckComponentHasCarsAndPedestrians(component);
            Grid.Components.Remove(component);
        }

        public void OpenFile(string path)
        {
            _gridContainer.Grid = _datalayer.OpenFile(path);
        }

        public void ResizeGrid(int w, int h)
        {
            if (Grid.Components.Any(x => x.X >= w * _config.GridWidth || x.Y >= h * _config.GridHeight))
            {
                throw new Exception("Components are outside of the grid");
            }
            Grid.Width = w;
            Grid.Height = h;
        }

        private static void CheckComponentHasCarsAndPedestrians(ComponentEntity component) // Some actions can not be performed when cars are on the crossroad
        {
            component.CarsInComponentLock.EnterReadLock();
            var carsInComponent = component.CarsInComponent.Any();
            component.CarsInComponentLock.ExitReadLock();
            if (carsInComponent)
                throw new Exception("Action can not  be performed with cars on the component.");

            var crossb = component as CrossroadBEntity;
            if (crossb != null)
            {
                crossb.PedestriansInComponentLock.EnterReadLock();
                var ped = crossb.PedestriansInComponent.Any();
                crossb.PedestriansInComponentLock.ExitReadLock();
                if (ped)
                    throw new Exception("Action can not  be performed with pedestrians on the component.");
            }

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
            CheckComponentHasCarsAndPedestrians(component);
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
