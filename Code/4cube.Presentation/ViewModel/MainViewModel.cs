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

        private readonly ObservableCollection<ComponentViewModel> _components = new ObservableCollection<ComponentViewModel>(); 

        public int Width { get; set; }
        public int Height { get; set; }
        public int ScaledWidth { get; set; }
        public int ScaledHeight { get; set; }
        public int Speed { get; set; } = 10;

        public MainViewModel() { }
        public MainViewModel(IGridModel gridModel, IConfig config, ISimulation simulation, GridContainer gridContainer)
        {
            _gridModel = gridModel;
            _simulation = simulation;
            _gridContainer = gridContainer;
            
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
                    break;
            }
        }

        private void LoadGrid(GridEntity grid)
        {
            GridItems.Clear();
            //GridItems.Add(new CollectionContainer {Collection = grid.Components });
            GridItems.Add(new CollectionContainer { Collection = _components });
            GridItems.Add(new CollectionContainer {Collection = grid.Cars});
            GridItems.Add(new CollectionContainer {Collection = grid.Pedestrians});
            BindingOperations.EnableCollectionSynchronization(grid.Cars, _lock);
            BindingOperations.EnableCollectionSynchronization(grid.Pedestrians, _lock);

            Width = grid.Width * _config.GridWidth;
            Height = grid.Width * _config.GridHeight;
            ScaledWidth = Width / _config.GetScale;
            ScaledHeight = Height / _config.GetScale;

            grid.PropertyChanged += GridOnPropertyChanged;
            grid.Components.CollectionChanged += ComponentsOnCollectionChanged;

            _components.Clear();
            foreach (var c in grid.Components)
            {
                AddComponent(c);
            }
        }
        
        private void AddComponent(ComponentEntity c)
        {
            _components.Add(new ComponentViewModel(c, _gridModel, _config));
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in notifyCollectionChangedEventArgs.NewItems)
                    {
                        AddComponent((ComponentEntity)c);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in notifyCollectionChangedEventArgs.OldItems)
                    {
                        var comp = _components.First(x => x.Component == c);
                        _components.Remove(comp);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _components.Clear();
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