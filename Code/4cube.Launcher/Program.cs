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
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Data;
using MainWindow = _4cube.Presentation.Window.MainWindow;

namespace _4cube.Launcher
{
    class Program 
    {
        [STAThread]
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<IGridModel>().To<GridModel>().InTransientScope();
            kernel.Bind<IGridData>().To<GridData>().InTransientScope();
            kernel.Bind<ISimulation>().To<Simulation>().InTransientScope();
            kernel.Bind<MainWindow>().To<MainWindow>().InTransientScope();
            kernel.Bind<IConfig>().To<Config>().InSingletonScope();

            var mainWindow = kernel.Get<MainWindow>();
            mainWindow.ShowDialog();
        }
    }
}
