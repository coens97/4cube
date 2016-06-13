using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public ObservableCollection<TrafficGroupModel> LightGroups { get; set; } =
            new ObservableCollection<TrafficGroupModel>();

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {
            Test = "hello";
            foreach (TrafficLightGroup group in Enum.GetValues(typeof(TrafficLightGroup)))
            {
                LightGroups.Add(new TrafficGroupModel
                {
                    Group = "Light group " + group,
                    Time =
                        gridModel.Grid.GreenLightTimeEntities.First(x => x.TrafficLightGroupSelected == group).Duration
                });
            }

        }
    }
    [ImplementPropertyChanged]
    public class TrafficGroupModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Group { get; set; }
        public int Time { get; set; }
    }
}
