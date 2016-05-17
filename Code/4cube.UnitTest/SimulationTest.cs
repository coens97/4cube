using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common.Ai;

namespace _4cube.UnitTest
{
    [TestClass]
    public class SimulationTest
    {
        private Simulation _simulation;
        private IKernel _container;


        [TestInitializeAttribute]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<IConfig>().To<Config>().InSingletonScope();
            _container.Bind<Simulation>().To<Simulation>().InTransientScope();
            _simulation = _container.Get<Simulation>();
        }

        [TestMethod]
        public void TestMoveCarToPoint()
        {
            var car = new CarEntity {X=0,Y=0};
            
            Assert.AreEqual(new Tuple<int,int>(0, 4), _simulation.MoveCarToPoint(car, new Tuple<int, int>(0, 100)));
            Assert.AreEqual(new Tuple<int, int>(4, 0), _simulation.MoveCarToPoint(car, new Tuple<int, int>(100, 0)));
            Assert.AreEqual(new Tuple<int, int>(0, -4), _simulation.MoveCarToPoint(car, new Tuple<int, int>(0, -100)));
            Assert.AreEqual(new Tuple<int, int>(-4, 0), _simulation.MoveCarToPoint(car, new Tuple<int, int>(-100, 0)));

            Assert.AreEqual(new Tuple<int, int>(2, 2), _simulation.MoveCarToPoint(car, new Tuple<int, int>(100, 100)));
        }
    }
}
