using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Common;
using _4cube.Common.Ai;
using _4cube.Common.Components;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CompositeCollection GridItems { get; set; } = new CompositeCollection();

        private IGridModel _gridModel;

        public MainViewModel() { }
        public MainViewModel(IGridModel gridModel)
        {
            _gridModel = gridModel;
            
            _gridModel.Grid = new GridEntity
            {
                Pedestrians = new ObservableCollection<PedestrianEntity>(),
                Components = new ObservableCollection<ComponentEntity>(),
                Cars = new ObservableCollection<CarEntity>(new[]
               {
                    new CarEntity { X = 100, Y = 200 },
                    new CarEntity { X = 20, Y = 40 },
                    new CarEntity { X = 350, Y = 100 }
                })
            };

            var grid = _gridModel.Grid;
            GridItems.Add(new CollectionContainer() {Collection = grid.Cars});
            GridItems.Add(new CollectionContainer() {Collection = grid.Pedestrians});
            GridItems.Add(new CollectionContainer() {Collection = grid.Components});
        }
    }
}