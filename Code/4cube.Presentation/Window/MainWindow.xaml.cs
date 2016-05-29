using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using _4cube.Bussiness;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;
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

        public MainWindow(MainViewModel viewModel, IGridModel gridModel)
        {
            InitializeComponent();

            _gridModel = gridModel;
            DataContext = viewModel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _resizeWindow.Show();
            _configWindow.Show();
        }

        private void BtnStartStop_Click(object sender, RoutedEventArgs e)
        {
            _gridModel.Grid.Cars.First().X += 5;
        }
    }
}
