using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using _4cube.Bussiness.Simulation;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using System.Collections.Generic;
using _4cube.Common;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.UnitTest
{
    [TestClass]
    public class SimulatorTest
    {
        private ISimulation _simulation;
        private IKernel _container;
        private GridEntity _grid;
        [TestInitialize]
        public void MyTestInitialize()
        {
            _container = new StandardKernel();
            _container.Bind<ISimulation>().To<Simulation>().InTransientScope();
            _simulation = _container.Get<ISimulation>();
            MakeTestGrid();
        }

        public void MakeTestGrid()
        {
            _grid = new GridEntity
            {
                Components = new List<ComponentEntity>(new ComponentEntity[] {                
                    new CurvedRoadEntity { X = 0, Y = 0, Rotation = Direction.Up, NrOfIncomingCars = new int[] { 4, 4, 0, 0} },
                    new CrossroadAEntity { X=5, Y = 0, Rotation=Direction.Right,NrOfIncomingCars=new int[] { 1,4,0,2}, CurrentGreenLightGroup=0,
                        GreenLightTimeEntities = new List<GreenLightTimeEntity>(new GreenLightTimeEntity[] {
                            new GreenLightTimeEntity { Duration = 5, TrafficLightGroup = TrafficLightGroup.A1 },
                            new GreenLightTimeEntity { Duration = 4, TrafficLightGroup = TrafficLightGroup.A2 },
                            new GreenLightTimeEntity { Duration = 3, TrafficLightGroup = TrafficLightGroup.A3 },
                            new GreenLightTimeEntity { Duration = 4, TrafficLightGroup = TrafficLightGroup.A4 }
                            }) },
                    new CrossroadBEntity { X=10, Y = 0, Rotation=Direction.Right,NrOfIncomingCars=new int[] { 1,5,1,0}, CurrentGreenLightGroup=0,
                        GreenLightTimeEntities = new List<GreenLightTimeEntity>(new GreenLightTimeEntity[] {
                            new GreenLightTimeEntity { Duration = 1, TrafficLightGroup = TrafficLightGroup.B1 },
                            new GreenLightTimeEntity { Duration = 3, TrafficLightGroup = TrafficLightGroup.B2 },
                            new GreenLightTimeEntity { Duration = 2, TrafficLightGroup = TrafficLightGroup.B3 },
                            new GreenLightTimeEntity { Duration = 4, TrafficLightGroup = TrafficLightGroup.B4 },
                            new GreenLightTimeEntity { Duration = 5, TrafficLightGroup = TrafficLightGroup.B5 }
                            }) }

                })
            };
        }
        /*
        [TestMethod]
        public void TestProcessTrafficLight()
        {
            _simulation.Start(_grid);
        }*/
    }
}
