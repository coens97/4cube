using System;
using System.ComponentModel;
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
        private ConfigurationModel _configurationModel;
        public ConfigurationWindow(IGridModel gridModel, ConfigurationModel configurationModel)
        {
            InitializeComponent();
            _gridModel = gridModel;
            _configurationModel = configurationModel;
            DataContext = _configurationModel;
            _configurationModel.PropertyChanged+= ConfigurationModelOnPropertyChanged;
        }

        private void ConfigurationModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            //_gridModel.Grid.
        }
    }
}
