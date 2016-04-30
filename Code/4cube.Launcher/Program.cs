using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using _4cube.Bussiness;
using _4cube.Presentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _4cube.Launcher
{
    class Program 
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var gridModel = kernel.Get<IGridModel>();
            var simulation = kernel.Get<ISimulation>();
            var mainWindow = kernel.Get<MainWindow>();

            mainWindow.Show();
        }
    }
}
