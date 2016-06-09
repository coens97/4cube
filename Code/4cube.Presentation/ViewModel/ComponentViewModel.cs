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
        
        public event PropertyChangedEventHandler PropertyChanged;

        [DoNotNotify]
        public ComponentEntity Component { get; set; }

        public ComponentViewModel() { }
        private IGridModel _gridModel;
        private IConfig _config;
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
            PropertyChanged += ViewOnPropertyChanged;
        }

        private void InitializeVisibility()
        {
            var lane = _config.GetLanesOfComponent(Component).Where(x => x.OutgoingDiretion.Any());

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
                    break;
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
