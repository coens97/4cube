using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
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
        private readonly GridResizingWindow _resizeWindow;
        private readonly System.Windows.Window _configWindow;
        private readonly IGridModel _gridModel;
        private readonly IConfig _config;
        private readonly ISimulation _simulation;
        private string _draggedComponent;
        private string _path = string.Empty;
        private MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel, IGridModel gridModel, IConfig config,
            ISimulation simulation, GridResizingWindow gridResizingWindow, ConfigurationWindow configurationWindow)
        {
            InitializeComponent();

            _gridModel = gridModel;
            _config = config;
            _simulation = simulation;
            DataContext = viewModel;
            _resizeWindow = gridResizingWindow;
            _configWindow = configurationWindow;
            _viewModel = viewModel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _configWindow.Show();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Running)
            {
                _viewModel.Running = false;
                _viewModel.Paused = false;
                _simulation.Stop();
            }
            else
            {
                _viewModel.Running = true;
                _simulation.Start(_gridModel.Grid);
            }
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
                    case "CrossroadB":
                        _gridModel.AddComponent(new CrossroadBEntity { X = position.Item1, Y = position.Item2 });
                        break;
                    case "RoadA":
                        _gridModel.AddComponent(new StraightRoadEntity { X = position.Item1, Y = position.Item2 });
                        break;
                    case "RoadB":
                        _gridModel.AddComponent(new CurvedRoadEntity { X = position.Item1, Y = position.Item2 });
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
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "TSIM Files (.tsim)|*.tsim|All Files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };

            var userClickedOk = openFileDialog1.ShowDialog();
            if (userClickedOk == true)
            {
                _path = openFileDialog1.FileName;
                _gridModel.OpenFile(openFileDialog1.FileName);
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "TSIM Files (.tsim)|*.tsim|All Files (*.*)|*.*",
                FilterIndex = 1
            };

            var userClickedOk = saveFileDialog1.ShowDialog();
            if (userClickedOk != null && userClickedOk.Value)
            {
                _path = saveFileDialog1.FileName;
                _gridModel.SaveFile(saveFileDialog1.FileName);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            _path = string.Empty;
            _gridModel.New();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            _resizeWindow.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (_path == string.Empty)
                MenuItem_Click_5(sender, e);
            else
            {
                _gridModel.SaveFile(_path);
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Paused)
            {
                _viewModel.Paused = false;
                _simulation.Start(_gridModel.Grid);
            }
            else
            {
                _viewModel.Paused = true;
                _simulation.Pause();
            }
        }
    }
}
