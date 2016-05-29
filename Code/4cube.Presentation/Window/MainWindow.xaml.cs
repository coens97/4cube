using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using _4cube.Bussiness;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;

namespace _4cube.Presentation.Window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly System.Windows.Window _resizeWindow = new GridResizingWindow();
        private readonly System.Windows.Window _configWindow = new ConfigurationWindow();
        private readonly IGridModel _gridModel;
        private readonly GridEntity _grid;
        public MainWindow(IGridModel gridModel)
        {
            InitializeComponent();
            _gridModel = gridModel;
            _grid = _gridModel.Grid;
            _grid = new GridEntity
            {
                Pedestrians = new ObservableCollection<PedestrianEntity>(),
                Components = new ObservableCollection<ComponentEntity>(),
                Cars = new ObservableCollection<CarEntity>(new []
                {
                    new CarEntity { X = 100, Y = 200 },
                    new CarEntity { X = 20, Y = 40 },
                    new CarEntity { X = 350, Y = 100 }
                })
            };
            var rand = new Random();
            //_grid.Cars.Add(new CarEntity {  X = rand.Next(50, 500), Y = rand.Next(50, 200) });
            //_grid.Cars.Add(new CarEntity {  X = rand.Next(50, 500), Y = rand.Next(50, 200) });
            //_grid.Cars.Add(new CarEntity {  X = rand.Next(50, 500), Y = rand.Next(50, 200) });
            DataContext = _grid;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _resizeWindow.Show();
            _configWindow.Show();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            //var rand = new Random();
            //_grid.Cars.Add(new CarEntity {Direction = Direction.Up, X = 10, Y =10});
            _grid.Cars.RemoveAt(0);
        }
    }
}
