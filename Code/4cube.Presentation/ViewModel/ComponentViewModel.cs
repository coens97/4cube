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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PropertyChanged;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;
using _4cube.Presentation.Annotations;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    class ComponentViewModel: INotifyPropertyChanged
    {
        public BitmapImage CompSource { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ObservableCollection<int> NrOfIncomingCars { get; set; } = new ObservableCollection<int>(new[] { 5, 7, 4, 3 });
        public ObservableCollection<Visibility> Enable { get; set; } = new ObservableCollection<Visibility>(new[] { Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Hidden });
        public event PropertyChangedEventHandler PropertyChanged;

        public ComponentEntity Component { get; set; }

        public ComponentViewModel() { }

        public ComponentViewModel(ComponentEntity c)
        {
            Component = c;
            X = c.X;
            Y = c.Y;
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
