using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _4cube.Bussiness;

namespace _4cube.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Window _resizeWindow = new GridResizingWindow();
        private readonly Window _configWindow = new Configuration();
        private readonly IGridModel _gridModel;
        public MainWindow(IGridModel gridModel)
        {
            InitializeComponent();
            _gridModel = gridModel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           _resizeWindow.Show();
            _configWindow.Show();
        }
    }
}
