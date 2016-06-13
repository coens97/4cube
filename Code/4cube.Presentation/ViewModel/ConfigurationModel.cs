using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class ConfigurationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Test { get; set; }
        public ObservableCollection<string> LightGroups { get; set; } = new ObservableCollection<string>();

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {
            Test = "hello";
            LightGroups.Add("aaaaa");
            LightGroups.Add("bbbbbb");
        }
    }
}
