using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4cube.Common.Ai;

namespace _4cube.UnitTest
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void TesViewModelNotify()
        {
            object send = null;
            PropertyChangedEventArgs arg = null;

            var ped = new PedestrianEntity {X = 100, Y = 200};
            ped.PropertyChanged += (sender, args) =>
            {
                send = sender;
                arg = args;
            };

            Assert.IsNull(send);

            ped.X = 150;

            Assert.AreEqual(send, ped);
            Assert.AreEqual(arg?.PropertyName, "X");
        }
    }
}