using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4cube.Data;
using _4cube.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;

namespace _4cube.UnitTest
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void TestSaveOpenFile()
        {
            var pds1 = new PedestrianEntity { Direction = Direction.Down, X = 5, Y = 18 };
            var pds2 = new PedestrianEntity { Direction = Direction.Left, X = 90, Y = 25 };
            
            var car1 = new CarEntity { Direction = Direction.Down, X = 17, Y = 40 };
            var car2 = new CarEntity { Direction = Direction.Right, X = 38, Y = 57 };

            var testgrid = new GridEntity
            {
                Pedestrians = new ObservableCollection<PedestrianEntity>(),
                Components = new ObservableCollection<ComponentEntity>(),
                Cars = new ObservableCollection<CarEntity>()
            };
            testgrid.Pedestrians.Add(pds1);
            testgrid.Pedestrians.Add(pds2);
           
            testgrid.Cars.Add(car1);
            testgrid.Cars.Add(car2);


            var gd = new GridData();
            var pth = "test.xml";
            gd.SaveFile(pth, testgrid);
            Assert.AreEqual(testgrid.Pedestrians.First().X, 5);
            Assert.AreEqual(testgrid.Cars.First().Y, 40);
            
            var testgrid2 = gd.OpenFile(pth);
            Assert.AreEqual(testgrid2.Pedestrians.First().X, 5);
            Assert.AreEqual(testgrid.Cars.First().Y, 40);
        }
    }
}
