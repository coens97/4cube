using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using _4cube.Bussiness;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.UnitTest
{
    [TestClass]
    public class GridModelTest
    {
        private IGridModel _gridModel;
        private IKernel _container;
        private GridEntity _grid;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<IGridModel>().To<GridModel>().InTransientScope();
            _gridModel = _container.Get<GridModel>();
            MakeTestGrid();
        }

        public void MakeTestGrid()
        {
            _grid = new GridEntity
            {
                Components = new List<ComponentEntity>(),
                Height = 300,
                Width = 400
            };
        }

        [TestMethod]
        public void TestAddandDeleteComponent()
        {
            int addExpected = 1;
            int deleteExpected = 0;
            int actual = _grid.Components.Count;
            ComponentEntity component = new ComponentEntity();

            _gridModel.AddComponent(component); 
            Assert.AreEqual(addExpected,actual);

            _gridModel.DeleteComponent(component);
            Assert.AreEqual(deleteExpected,actual);           
        }

        public void TestRotateComponennt()
        {
            ComponentEntity component = new ComponentEntity
            {
                Rotation = Direction.Up
            };
            Direction expected = Direction.Right;
            Direction actual = component.Rotation;

            _gridModel.RotateComponent(component);
            Assert.AreEqual(expected,actual);
        }

        public void TestResizeGrid()
        {
            double widthExpected = 500;
            double heightExpected = 400;
            double widthActual = _grid.Width;
            double heightActual = _grid.Height;

            _gridModel.ResizeGrid(500,400);
            Assert.AreEqual(widthExpected,widthActual);
            Assert.AreEqual(heightExpected,heightActual);
        }

    }
}
