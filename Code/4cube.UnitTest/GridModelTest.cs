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
using _4cube.Data;

namespace _4cube.UnitTest
{
    [TestClass]
    public class GridModelTest
    {
        private GridModel _gridModel;
        private IKernel _container;




        [TestInitialize]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<IGridModel>().To<GridModel>().InTransientScope();
            _container.Bind<IGridData>().To<GridData>().InTransientScope();
            _gridModel = _container.Get<GridModel>();

        }


        [TestMethod]
        public void TestAddandDeleteComponent()
        {

            int[] testa = { 2, 3, 4 };

            ComponentEntity component = new ComponentEntity { NrOfIncomingCars = testa, Rotation = Direction.Down, X = 11, Y = 33 };

            _gridModel.AddComponent(component);

            Assert.AreEqual(_gridModel._grid.Components.Count, 1);

            _gridModel.DeleteComponent(component);
            Assert.AreEqual(_gridModel._grid.Components.Count, 0);
        }

        [TestMethod]
        public void TestRotateComponennt()
        {
            ComponentEntity component = new ComponentEntity
            {
                Rotation = Direction.Up
            };
            Direction expected = Direction.Right;


            _gridModel.RotateComponent(component);
            Assert.AreEqual(component.Rotation, expected);
        }

        [TestMethod]
        public void TestResizeGrid()
        {

            _gridModel.ResizeGrid(500, 400);
            Assert.AreEqual(_gridModel._grid.Width, 500);
            Assert.AreEqual(_gridModel._grid.Height, 400);
        }

        [TestMethod]

        public void TestGreenlight()
        {
            GreenLightTimeEntity glt = new GreenLightTimeEntity { Duration = 1, TrafficLightGroup = TrafficLightGroup.A4 };

            TrafficLightGroup testtl = TrafficLightGroup.A4;

            _gridModel.GreenLight(glt, testtl, 20);

            Assert.AreEqual(glt.Duration, 20);
            Assert.AreEqual(glt.TrafficLightGroup, TrafficLightGroup.A4);

        }

    }
}
