using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.TrafficLight;

namespace _4cube.Common
{
    [ImplementPropertyChanged]
    public class GridEntity : INotifyPropertyChanged
    {
        public ObservableCollection<CarEntity> Cars { get; set; } = new ObservableCollection<CarEntity>();

        public ObservableCollection<ComponentEntity> Components { get; set; } =
            new ObservableCollection<ComponentEntity>();

        public int Height { get; set; } = 8;

        public ObservableCollection<PedestrianEntity> Pedestrians { get; set; } =
            new ObservableCollection<PedestrianEntity>();
        public GreenLightTimeEntity[] GreenLightTimeEntities { get; set; } =  
        {
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.A1},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.A2},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.A3},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.A4},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.B1},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.B2},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.B3},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.B4},
            new GreenLightTimeEntity { TrafficLightGroupSelected = TrafficLightGroup.B5}
        };

        public int Width { get; set; } = 12;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
