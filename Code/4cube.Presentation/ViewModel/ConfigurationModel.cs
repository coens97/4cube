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

        public TrafficGroupModel SelectedProp {get; set; }
        private IGridModel _gridModel;

        public ObservableCollection<TrafficGroupModel> LightGroups { get; set; } =
            new ObservableCollection<TrafficGroupModel>();

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {
            _gridModel = gridModel;
            foreach (TrafficLightGroup group in Enum.GetValues(typeof(TrafficLightGroup)))
            {
                var model = new TrafficGroupModel
                {
                    Group = "Light group " + group,
                    Time =
                        gridModel.Grid.GreenLightTimeEntities.First(x => x.TrafficLightGroupSelected == group).Duration
                };
                model.PropertyChanged += ModelOnPropertyChanged;
                LightGroups.Add(model);
            }

        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var model = sender as TrafficGroupModel;
            _gridModel.Grid.GreenLightTimeEntities.First(x => x.TrafficLightGroupSelected == model.TrafficLightGroup)
                .Duration = model.Time;
        }
    }
    [ImplementPropertyChanged]
    public class TrafficGroupModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TrafficLightGroup TrafficLightGroup { get; set; }
        public string Group { get; set; }
        public int Time { get; set; }
    }
}
