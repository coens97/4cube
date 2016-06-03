using System.Collections.Generic;
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
        public int Height { get; set; }

        public ObservableCollection<PedestrianEntity> Pedestrians { get; set; } =
            new ObservableCollection<PedestrianEntity>();
        public ObservableCollection<GreenLightTimeEntity> GreenLightTimeEntities { get; set; } = new ObservableCollection<GreenLightTimeEntity>(new []
        {
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.A1},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.A2},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.A3},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.A4},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.B1},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.B2},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.B3},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.B4},
            new GreenLightTimeEntity { Duration = 5, TrafficLightGroupSelected = TrafficLightGroup.B5}
        });
        public int Width { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
