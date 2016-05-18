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
            Assert.IsTrue(carOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1],0, 0, _config.GridWidth, _config.GridHeight, Direction.Up));
            Assert.IsFalse(carNotOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], 0, 0, _config.GridWidth, _config.GridHeight, Direction.Up));
        }

        [TestMethod]
        public void TestIsInPositionWithGrid()
        {
            var gridX = 800;
            var gridY = 400;
            var carOnLane = new CarEntity { X = gridX + 125, Y = gridY + 5 }; // car on lane a1
            var carNotOnLane = new CarEntity { X = gridX + 120, Y = gridY + 180 }; // car not on lane a1
            Assert.IsTrue(carOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], gridX, gridY, _config.GridWidth, _config.GridHeight, Direction.Up));
            Assert.IsFalse(carNotOnLane.IsInPosition(_config.CrossRoadCoordinatesCars[TrafficLightGroup.A1], gridX, gridY, _config.GridWidth, _config.GridHeight, Direction.Up));
        }

        [TestMethod]
        public void TestRotate()
        {
            Tuple<int, int> testeTupleA;
            Tuple<int, int> testTuple2 = new Tuple<int, int>(300,100);
            testeTupleA = SimulationUtility.Rotate(100, 100, 200, 200, Direction.Right);

            Assert.AreEqual(testeTupleA,testTuple2);

            Tuple<int, int> testTupleB;
            Tuple<int, int> testTuple3 = new Tuple<int, int>(250, 100);

            Tuple<int, int> testTupleC;
            Tuple<int,int> testTuple4 = new Tuple<int, int>(300,250);

            testTupleB = SimulationUtility.Rotate(100, 150, 200, 200, Direction.Right);
            testTupleC = SimulationUtility.Rotate(100, 150, 200, 200, Direction.Down);

            Assert.AreEqual(testTupleB,testTuple3);
            Assert.AreEqual(testTupleC,testTuple4);


        }

        [TestMethod]
        public void TestGetGridPosition()
        {
            // from an input x and y get the position of the lefttop corner of the grid it is in
            // width = 400 height = 400 then (100,100)->(0,0);420,20->400,0
            var width = 400;
            var height = 400;
            Assert.AreEqual(new Tuple<int,int>(0,0),SimulationUtility.GetGridPosition(200,200,width,height) );
            Assert.AreEqual(new Tuple<int, int>(0, 0), SimulationUtility.GetGridPosition(399, 399, width, height));
            Assert.AreEqual(new Tuple<int, int>(400, 400), SimulationUtility.GetGridPosition(400, 400, width, height));
            Assert.AreEqual(new Tuple<int, int>(800, 400), SimulationUtility.GetGridPosition(885, 445, width, height));
        }


        [TestMethod]

        public void TestRotateTuples()
        {
            var tuples = _config.GetAllLanesOfTrafficLight(typeof(CrossroadAEntity));

            var testTuples = SimulationUtility.Rotate(tuples, 0, 0, _config.GridWidth, _config.GridHeight, Direction.Down);
            var testTuples0 = SimulationUtility.Rotate(tuples,-200, -200, _config.GridWidth, _config.GridHeight, Direction.Right);
            
            Assert.AreEqual(testTuples[0],new Tuple<int, int, int, int>(278,400,228,278));
            Assert.AreEqual(testTuples0[2], new Tuple<int, int, int, int>(0,222,-121,278));// Assert.AreEqual(testTuples0[2], new Tuple<int, int, int, int>(0,222,-122,278));



        }

        [TestMethod]
        public void TestRotateDirection()
        {
            Assert.AreEqual(Direction.Right, Direction.Right.RotatedDirection(Direction.Up));
            Assert.AreEqual(Direction.Up, Direction.Down.RotatedDirection(Direction.Down));
            Assert.AreEqual(Direction.Down, Direction.Left.RotatedDirection(Direction.Left));
        }
    }
}
