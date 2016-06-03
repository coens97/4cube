using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Data;
using _4cube.Presentation.Window;

namespace _4cube.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this._container = new StandardKernel();
            _container.Bind<IGridModel>().To<GridModel>().InSingletonScope();
            _container.Bind<IGridData>().To<GridData>().InTransientScope();
            _container.Bind<ISimulation>().To<Simulation>().InSingletonScope();
            _container.Bind<MainWindow>().ToSelf().InTransientScope();
            _container.Bind<GridContainer>().ToSelf().InSingletonScope();
            _container.Bind<IConfig>().To<Config>().InSingletonScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this._container.Get<MainWindow>();
        }
    }
}
