﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using _4cube.Bussiness;
using _4cube.Common;
using _4cube.Common.Components;
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
            var component = new ComponentEntity { Rotation = Direction.Down, X = 11, Y = 33 };

            _gridModel.AddComponent(component);

            Assert.AreEqual(_gridModel.Grid.Components.Count, 1);

            _gridModel.DeleteComponent(component);
            Assert.AreEqual(_gridModel.Grid.Components.Count, 0);
        }

        [TestMethod]
        public void TestRotateComponennt()
        {
            var component = new ComponentEntity
            {
                Rotation = Direction.Up
            };
            var expected = Direction.Right;

            Assert.AreEqual(component.Rotation, Direction.Up);
            _gridModel.RotateComponent(component);
            Assert.AreEqual(component.Rotation, expected);
        }

        [TestMethod]
        public void TestResizeGrid()
        {

            _gridModel.ResizeGrid(500, 400);
            Assert.AreEqual(_gridModel.Grid.Width, 500);
            Assert.AreEqual(_gridModel.Grid.Height, 400);
        }
    }
}
