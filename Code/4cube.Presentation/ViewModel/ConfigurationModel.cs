using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Common.Components.TrafficLight;
using _4cube.Presentation.Annotations;

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
