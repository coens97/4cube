using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4cube.Bussiness.Simulation;
using Ninject;
using _4cube.Common.Ai;

namespace _4cube.UnitTest
{
    [TestClass]
    public class SimulationUtilityTest
    {
        private static SimulationUtility _simulationUtility;
        private CarEntity car1 = new CarEntity {X = 150, Y = 50};
        private CarEntity car2 = new CarEntity {X = 190,Y= 140};
        

        [TestMethod]
        public void TestIsInPosition()
        {

           bool carInPosition=_simulationUtility.IsInPosition(car1,new Tuple<int, int, int, int>(122, 0, 172, 122));
            Assert.AreEqual(carInPosition,true);

            bool carInPosition = _simulationUtility.IsInPosition(car2, new Tuple<int, int, int, int>(122, 0, 172, 122));
            Assert.AreEqual(carInPosition, false);
        }
    }
}
