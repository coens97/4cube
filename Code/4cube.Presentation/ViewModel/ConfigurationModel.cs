using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class ConfigurationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //  public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }
        public ObservableCollection<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {
            GreenLightTimeEntities = gridModel.Grid.GreenLightTimeEntities;
        }
    }
}
