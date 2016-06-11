using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Presentation.Annotations;
using _4cube.Presentation.UserControl;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    class ComponentViewModel: INotifyPropertyChanged
    {
        public int Rotation { get; set; }
        public BitmapImage CompSource { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TopCars { get; set; }
        public int RightCars { get; set; } 
        public int LeftCars { get; set; } 
        public int BotCars { get; set; } 
        public Visibility TopVisibility { get; set; } = Visibility.Hidden;
        public Visibility RightVisibility { get; set; } = Visibility.Hidden;
        public Visibility LeftVisibility { get; set; } = Visibility.Hidden;
        public Visibility BotVisibility { get; set; } = Visibility.Hidden;
        public ObservableCollection<TrafficLightComponent> Lights { get; set; } = new ObservableCollection<TrafficLightComponent>();
        public event PropertyChangedEventHandler PropertyChanged;

        [DoNotNotify]
        public ComponentEntity Component { get; set; }

        public ComponentViewModel() { }
        private readonly IGridModel _gridModel;
        private readonly IConfig _config;
        public ComponentViewModel(ComponentEntity c, IGridModel gridModel, IConfig config)
        {
            _config = config;
            _gridModel = gridModel;
            Component = c;
            X = c.X;
            Y = c.Y;
            Rotation = (int)c.Rotation * 90;
            c.PropertyChanged += COnPropertyChanged;
            TopCars = Component.NrOfIncomingCars[0];
            RightCars = Component.NrOfIncomingCars[1];
            LeftCars = Component.NrOfIncomingCars[3];
            BotCars = Component.NrOfIncomingCars[2];

            var p = AssemblyDirectory;
            if (c is CrossroadAEntity)
            {
                CompSource = new BitmapImage(new Uri(p + "/Resources/A.png", UriKind.Absolute));
            }
            else if (c is CrossroadBEntity)
            {
                CompSource = new BitmapImage(new Uri(p + "/Resources/B.png", UriKind.Absolute));
            }
            else if (c is StraightRoadEntity)
            {
                CompSource = new BitmapImage(new Uri(p + "/Resources/roada.png", UriKind.Absolute));
            }
            else
            {
                CompSource = new BitmapImage(new Uri(p + "/Resources/roadb.png", UriKind.Absolute));
            }
            InitializeVisibility();

            PlaceLights();
            PropertyChanged += ViewOnPropertyChanged;
        }

        private void InitializeVisibility()
        {
            var lane = _config.GetLanesOfComponent(Component).Where(x => x.OutgoingDirection.Any());

            TopVisibility = lane.Any(x => x.DirectionLane == Component.Rotation.RotatedDirectionInv(Direction.Left)) ? Visibility.Visible : Visibility.Hidden;
            RightVisibility = lane.Any(x => x.DirectionLane == Component.Rotation.RotatedDirectionInv(Direction.Up)) ? Visibility.Visible : Visibility.Hidden;
            BotVisibility = lane.Any(x => x.DirectionLane == Component.Rotation.RotatedDirectionInv(Direction.Right)) ? Visibility.Visible : Visibility.Hidden;
            LeftVisibility = lane.Any(x => x.DirectionLane == Component.Rotation.RotatedDirectionInv(Direction.Down)) ? Visibility.Visible : Visibility.Hidden;
            
        }

        private void ViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TopCars":
                    Component.NrOfIncomingCars[0] = TopCars;
                    break;
                case "RightCars":
                    Component.NrOfIncomingCars[1] = RightCars;
                    break;
                case "LeftCars":
                    Component.NrOfIncomingCars[3] = LeftCars;
                    break;
                case "BotCars":
                    Component.NrOfIncomingCars[2] = BotCars;
                    break;
            }
        }

        public void OnRotate()
        {
            try
            {
                _gridModel.RotateComponent(Component);
                TopCars = Component.NrOfIncomingCars[0];
                RightCars = Component.NrOfIncomingCars[1];
                LeftCars = Component.NrOfIncomingCars[3];
                BotCars = Component.NrOfIncomingCars[2];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void OnDelete()
        {
            try
            {
                _gridModel.DeleteComponent(Component);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void COnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Rotation":
                    Rotation = (int)Component.Rotation * 90;
                    InitializeVisibility();
                    PlaceLights();
                    break;
                case "CurrentGreenLightGroup":
                    ChangeTrafficColors();
                    break;
                case "LightOrange":
                    ChangeTrafficColors();
                    break;
            }
        }

        private void PlaceLights()
        {
            if (!(Component is CrossroadEntity))
                return;
            Lights.Clear();;
            var lane = _config.GetLanesOfComponent(Component).Where(x => x.OutgoingDirection.Any());
            foreach (var l in lane)
            {
                var point = l.ExitPoint.Rotate(Component.Rotation, _config.GridWidth, _config.GridHeight);
                Lights.Add(new TrafficLightComponent
                {
                    X = point.Item1,
                    Y = point.Item2,
                    Color = System.Windows.Media.Brushes.Red,
                    Lane = l
                });
            }
            ChangeTrafficColors();
        }

        private void ChangeTrafficColors()
        {
            var crossroad = Component as CrossroadEntity;
            var currentGroup = crossroad.GreenLightTimeEntities[crossroad.CurrentGreenLightGroup];
            var lane = _config.CrossRoadCoordinatesCars[currentGroup];
            foreach (var l in Lights)
            {
                l.Color = System.Windows.Media.Brushes.Red;
                if (lane.Any(x=> x.Equals(l.Lane)))
                {
                    l.Color = crossroad.LightOrange ? System.Windows.Media.Brushes.Orange : System.Windows.Media.Brushes.Green;
                }
            }
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
}
