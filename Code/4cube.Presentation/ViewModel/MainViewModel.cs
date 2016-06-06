using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
        private static object _lock = new object();
        public event PropertyChangedEventHandler PropertyChanged;

        public CompositeCollection GridItems { get; set; } = new CompositeCollection();

        private IGridModel _gridModel;
        private ISimulation _simulation;
        private IConfig _config;
        private GridContainer _gridContainer;

        private ObservableCollection<ComponentViewModel> Components = new ObservableCollection<ComponentViewModel>(); 

        public int Width { get; set; }
        public int Height { get; set; }
        public int ScaledWidth { get; set; }
        public int ScaledHeight { get; set; }
        public int Speed { get; set; }

        public MainViewModel() { }
        public MainViewModel(IGridModel gridModel, IConfig config, ISimulation simulation, GridContainer gridContainer)
        {
            _gridModel = gridModel;
            _simulation = simulation;
            _gridContainer = gridContainer;
            
            // Test data.. should be deleted later
            gridContainer.Grid = new GridEntity
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
            _config = config;

            BindingOperations.EnableCollectionSynchronization(GridItems, _lock);
            LoadGrid(_gridContainer.Grid);

            _gridContainer.PropertyChanged += GridContainerOnPropertyChanged;

            this.PropertyChanged += OnPropertyChanged;
        }

        private void GridContainerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Grid":
                    LoadGrid(_gridContainer.Grid);
                    Components.Clear();
                    break;
            }
        }

        void LoadGrid(GridEntity grid)
        {
            GridItems.Clear();
            //GridItems.Add(new CollectionContainer {Collection = grid.Components });
            GridItems.Add(new CollectionContainer {Collection = grid.Cars});
            GridItems.Add(new CollectionContainer {Collection = grid.Pedestrians});
            GridItems.Add(new CollectionContainer { Collection = Components });
            BindingOperations.EnableCollectionSynchronization(grid.Cars, _lock);
            BindingOperations.EnableCollectionSynchronization(grid.Pedestrians, _lock);

            Width = grid.Width * _config.GridWidth;
            Height = grid.Width * _config.GridHeight;
            ScaledWidth = Width / _config.GetScale;
            ScaledHeight = Height / _config.GetScale;

            grid.PropertyChanged += GridOnPropertyChanged;
            grid.Components.CollectionChanged += ComponentsOnCollectionChanged;
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in notifyCollectionChangedEventArgs.NewItems)
                    {
                        Components.Add(new ComponentViewModel((ComponentEntity)c, _gridModel));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in notifyCollectionChangedEventArgs.OldItems)
                    {
                        var comp = Components.First(x => x.Component == c);
                        Components.Remove(comp);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Components.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Speed":
                    _simulation.ChangeSpeed((10 - Speed)*4 + 16);
                    break;
            }
        }

        private void GridOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Width":
                    Width = _gridModel.Grid.Width*_config.GridWidth;
                    ScaledWidth = Width/_config.GetScale;
                    break;
                case "Height":
                    Height = _gridModel.Grid.Height*_config.GridHeight;
                    ScaledHeight = Height/_config.GetScale;
                    break;
            }
        }
    }
}