using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Presentation.ViewModel;

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
        private readonly IConfig _config;
        private readonly ISimulation _simulation;
        private string _draggedComponent;

        public MainWindow(MainViewModel viewModel, IGridModel gridModel, IConfig config, ISimulation simulation)
        {
            InitializeComponent();

            _gridModel = gridModel;
            _config = config;
            _simulation = simulation;
            DataContext = viewModel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _resizeWindow.Show();
            _configWindow.Show();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            //_gridModel.Grid.Cars.First().X += 5;
            _simulation.Start(_gridModel.Grid);
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image == null)
                return;
            _draggedComponent = image.Name;
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(string.IsNullOrEmpty(_draggedComponent))
                return;
            var canvas = sender as Canvas;
            var mouse = e.GetPosition(canvas);
            var position = SimulationUtility.GetGridPosition((int)mouse.X, (int)mouse.Y, _config.GridWidth, _config.GridHeight);
            try
            {
                switch (_draggedComponent)
                {
                    case "CrossroadA":
                        _gridModel.AddComponent(new CrossroadAEntity {X = position.Item1, Y = position.Item2});
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // TODO: Add a dialogbox to open file
            _gridModel.OpenFile("test.tsim");
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            // TODO: Add a dialogbox to save file
            _gridModel.SaveFile("test.tsim");
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            _gridModel.New();
        }
    }
}
