using System;
using System.ComponentModel;
using System.Linq;
using Ninject.Infrastructure.Language;
using _4cube.Bussiness;
using _4cube.Presentation.ViewModel;

namespace _4cube.Presentation.Window
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class ConfigurationWindow : System.Windows.Window
    {
        private IGridModel _gridModel;
        private readonly ConfigurationModel _configurationModel;
        public ConfigurationWindow(IGridModel gridModel, ConfigurationModel configurationModel)
        {
            InitializeComponent();
            _gridModel = gridModel;
            _configurationModel = configurationModel;
            DataContext = _configurationModel;
        }

        private void BtnChangeAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_configurationModel.SelectedProp == null) return;
            foreach (var x in _gridModel.Grid.GreenLightTimeEntities)
                x.Duration = _configurationModel.SelectedProp.Time;

            foreach (var x in _configurationModel.LightGroups)
            {
                x.Time = _configurationModel.SelectedProp.Time;
            }
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
