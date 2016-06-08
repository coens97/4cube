using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Common.Components.TrafficLight;
using System.Collections.ObjectModel;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class ConfigurationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //  public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }
        public CompositeCollection GreenLightTimeEntities { get; set; }

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {

            GreenLightTimeEntities.Add(new CollectionContainer{ Collection = gridModel.Grid.GreenLightTimeEntities} );
        }
    }
}
