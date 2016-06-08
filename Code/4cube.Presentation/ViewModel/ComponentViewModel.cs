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

        // observable collection is useless for getting notified when a value changes
        public ObservableCollection<int> NrOfIncomingCars { get; set; } = new ObservableCollection<int>(new[] { 5, 7, 4, 3 });
        public ObservableCollection<Visibility> Enable { get; set; } = new ObservableCollection<Visibility>(new[] { Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Hidden });
        public event PropertyChangedEventHandler PropertyChanged;

        public ComponentEntity Component { get; set; }

        public ComponentViewModel() { }
        private IGridModel _gridModel;
        public ComponentViewModel(ComponentEntity c, IGridModel gridModel)
        {
            _gridModel = gridModel;
            Component = c;
            X = c.X;
            Y = c.Y;
            Rotation = (int)c.Rotation * 90;

            c.PropertyChanged += COnPropertyChanged;
            this.PropertyChanged += OnPropertyChanged;

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

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "NrCTop":
                     
                    break;
            }
        }

        public void OnRotate()
        {
            _gridModel.RotateComponent(Component);
        }

        public void OnDelete()
        {
            _gridModel.DeleteComponent(Component);
        }

        private void COnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Rotation":
                    Rotation = (int)Component.Rotation * 90;
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
