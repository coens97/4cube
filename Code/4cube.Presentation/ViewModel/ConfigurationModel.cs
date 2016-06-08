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

        public CompositeCollection GreenLightTimeEntities { get; set; } =new CompositeCollection();
        public string Test { get; set; }
        public ObservableCollection<string>TestCollection=new ObservableCollection<string>();

        public ConfigurationModel()
        {
        }

        public ConfigurationModel(IGridModel gridModel)
        {
            Test = "hello";
            TestCollection.Add("aaaaa");
            TestCollection.Add("bbbbbb");
            GreenLightTimeEntities.Add(new CollectionContainer{ Collection = gridModel.Grid.GreenLightTimeEntities} );
        }
    }
}
