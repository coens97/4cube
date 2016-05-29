using System.Windows;
using _4cube.Bussiness;

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
