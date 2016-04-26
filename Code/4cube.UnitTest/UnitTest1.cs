using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4cube.Data;
using _4cube.Common;
using System.Collections.Generic;
using System.Linq;

namespace _4cube.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSaveOpenFile()
        {
            
            PedestrianEntity pds1 = new PedestrianEntity { Direction = Direction.Down, X = 5, Y = 18 };
            PedestrianEntity pds2 = new PedestrianEntity { Direction = Direction.Left, X = 90, Y = 25 };
            
            CarEntity car1 = new CarEntity { Direction = Direction.Down, X = 17, Y = 40 };
            CarEntity car2 = new CarEntity { Direction = Direction.Right, X = 38, Y = 57 };


            GridEntity testgrid = new GridEntity
            {
                Pedestrians = new List<PedestrianEntity>(),
                Components = new List<ComponentEntity>(),
                Cars = new List<CarEntity>()
               
            };
            testgrid.Pedestrians.Add(pds1);
            testgrid.Pedestrians.Add(pds2);
           
            testgrid.Cars.Add(car1);
            testgrid.Cars.Add(car2);


            IGridData gd = new GridData();
            string pth = "test.xml";
            gd.SaveFile(pth, testgrid);
            Assert.AreEqual(testgrid.Pedestrians.First().X, 5);
            Assert.AreEqual(testgrid.Cars.First().Y, 40);
            

            
            
            GridEntity testgrid2 = gd.OpenFile(pth);
            Assert.AreEqual(testgrid2.Pedestrians.First().X, 5);
            Assert.AreEqual(testgrid.Cars.First().Y, 40);
           



        }
    }
}
