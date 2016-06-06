using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using PropertyChanged;
using _4cube.Common.Components;
using _4cube.Presentation.Annotations;
using _4cube.Presentation.ViewModel;

namespace _4cube.Presentation.UserControl
{
    /// <summary>
    /// Interaction logic for ComponentControl.xaml
    /// </summary>
    
    public partial class ComponentControl : System.Windows.Controls.UserControl
    {
        public ComponentControl()
        {
            InitializeComponent();
        }

        private void Grid_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            var contextMenu = grid.ContextMenu;
            contextMenu.PlacementTarget = grid;
            contextMenu.IsOpen = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((ComponentViewModel)DataContext).OnRotate();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ((ComponentViewModel)DataContext).OnDelete();
        }
    }
} 