using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Bussiness.Simulation;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CompositeCollection GridItems { get; set; } = new CompositeCollection();

        private IGridModel _gridModel;
        private ISimulation _simulation;
        private IConfig _config;

        public int Width { get; set; }
        public int Height { get; set; }
        public int ScaledWidth { get; set; }
        public int ScaledHeight { get; set; }
        public int Speed { get; set; }

        public MainViewModel() { }
        public MainViewModel(IGridModel gridModel, IConfig config, ISimulation simulation)
        {
            _gridModel = gridModel;
            _simulation = simulation;
            
            // Test data.. should be deleted later
            _gridModel.Grid = new GridEntity
            {
                Pedestrians = new ObservableCollection<PedestrianEntity>(new[]
                {
                    new PedestrianEntity { X = 160, Y = 50 },
                    new PedestrianEntity { X = 160, Y = 240 }
                }),
                Components = new ObservableCollection<ComponentEntity>(new[]
                {
                    new CrossroadAEntity { X = 400, Y= 400 }
                }),
                Cars = new ObservableCollection<CarEntity>(new[]
               {
                    new CarEntity { X = 100, Y = 200 },
                    new CarEntity { X = 20, Y = 40 },
                    new CarEntity { X = 350, Y = 100 }
                }),
                Width = 10,
                Height = 8
            };

            var grid = _gridModel.Grid;
            GridItems.Add(new CollectionContainer() {Collection = grid.Cars});
            GridItems.Add(new CollectionContainer() {Collection = grid.Pedestrians});
            GridItems.Add(new CollectionContainer() {Collection = grid.Components});

            _config = config;

            Width = grid.Width * config.GridWidth;
            Height = grid.Width * config.GridHeight;
            ScaledWidth = Width/config.GetScale;
            ScaledHeight = Height/config.GetScale;

            grid.PropertyChanged += GridOnPropertyChanged;

            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Speed":
                    _simulation.ChangeSpeed((10 - Speed) * 4 + 16);
                    break;
            }
        }

        private void GridOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Width":
                    Width = _gridModel.Grid.Width * _config.GridWidth;
                    ScaledWidth = Width / _config.GetScale;
                    break;
                case "Height":
                    Height = _gridModel.Grid.Height * _config.GridHeight;
                    ScaledHeight = Height / _config.GetScale;
                    break;
            }
        }
    }
}