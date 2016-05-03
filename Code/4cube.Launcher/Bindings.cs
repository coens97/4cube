using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Presentation;
using _4cube.Bussiness.Simulation;

namespace _4cube.Launcher
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IGridModel>().To<GridModel>().InTransientScope();
            Bind<ISimulation>().To<Simulation>().InTransientScope();
            Bind<MainWindow>().To<MainWindow>().InTransientScope();
            Bind<IConfig>().To<Config>().InSingletonScope();
        }
    }
}
