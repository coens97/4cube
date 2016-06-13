using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class ConfigurationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage ImageSource { get; set; }
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
                    TrafficLightGroup = group,
                    Group = "Light group " + group,
                    Time =
                        gridModel.Grid.GreenLightTimeEntities.First(x => x.TrafficLightGroupSelected == group).Duration
                };
                model.PropertyChanged += ModelOnPropertyChanged;
                LightGroups.Add(model);
            }
            this.PropertyChanged += ConfigurationOnPropertyChanged;
        }

        private void ConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "SelectedProp")
            {
                var p = AssemblyDirectory;
                ImageSource = new BitmapImage(new Uri(p + "/Resources/"+ SelectedProp.TrafficLightGroup +".png", UriKind.Absolute));
            }
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var model = sender as TrafficGroupModel;
            _gridModel.Grid.GreenLightTimeEntities.First(x => x.TrafficLightGroupSelected == model.TrafficLightGroup)
                .Duration = model.Time;
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
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
