using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common.Ai;
using _4cube.Common.Components.Crossroad;

namespace _4cube.UnitTest
{
    [TestClass]
    public class SimulationTest
    {
        private Simulation _simulation;
        private IConfig _config;
        private IKernel _container;


        [TestInitializeAttribute]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<IConfig>().To<Config>().InSingletonScope();
            _container.Bind<Simulation>().To<Simulation>().InTransientScope();
            _simulation = _container.Get<Simulation>();
            _config = _container.Get<IConfig>();
        }

        [TestMethod]
        public void TestMoveCarToPoint()
        {
            var cross = new CrossroadAEntity {X = 0, Y = 0};
            var car = new CarEntity {X=0,Y=0};
            var d = _config.CarSpeed;
            
            Assert.AreEqual(new Tuple<int,int>(0, d), Simulation.MoveCarToPoint(car, new Tuple<int, int>(0, 100), cross, d));
            Assert.AreEqual(new Tuple<int, int>(d, 0), Simulation.MoveCarToPoint(car, new Tuple<int, int>(100, 0), cross, d));
            Assert.AreEqual(new Tuple<int, int>(0, -d), Simulation.MoveCarToPoint(car, new Tuple<int, int>(0, -100), cross, d));
            Assert.AreEqual(new Tuple<int, int>(-d, 0), Simulation.MoveCarToPoint(car, new Tuple<int, int>(-100, 0), cross, d));

            var deg45 = (int)Math.Sqrt((d*d)/2);
            Assert.AreEqual(new Tuple<int, int>(deg45, deg45), Simulation.MoveCarToPoint(car, new Tuple<int, int>(100, 100), cross, d));
        }
    }
}
