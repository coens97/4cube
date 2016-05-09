using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4cube.Bussiness.Simulation;
using Ninject;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Common.Components.TrafficLight;
using _4cube.Data;

namespace _4cube.UnitTest
{
    [TestClass]
    public class SimulationUtilityTest
    {

        private IConfig _config;
        private IKernel _container;


        [TestInitializeAttribute]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<IConfig>().To<Config>().InSingletonScope();
            _config = _container.Get<IConfig>();

        }

        [TestMethod]
        public void TestIsInPositionNoGrid()
        {
            var carOnLane = new CarEntity {X = 125, Y = 5}; // car on lane a1
            var carNotOnLane = new CarEntity { X = 120, Y = 180 }; // car not on lane a1
            Assert.IsTrue(carOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1],0, 0));
            Assert.IsFalse(carNotOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], 0, 0));
        }

        [TestMethod]
        public void TestIsInPositionWithGrid()
        {
            var gridX = 800;
            var gridY = 400;
            var carOnLane = new CarEntity { X = gridX + 125, Y = gridY + 5 }; // car on lane a1
            var carNotOnLane = new CarEntity { X = gridX + 120, Y = gridY + 180 }; // car not on lane a1
            Assert.IsTrue(carOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], gridX, gridY));
            Assert.IsFalse(carNotOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], gridX, gridY));
        }
    }
}
