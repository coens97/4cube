using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Common.Components.TrafficLight;

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
            foreach (var group in Enum.GetValues(typeof(TrafficLightGroup)))
            {
                LightGroups.Add("Light group " + group);
            }
            
        }
    }
}
