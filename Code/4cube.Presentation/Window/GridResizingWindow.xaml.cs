using System;
using System.ComponentModel;
using _4cube.Bussiness;
using _4cube.Presentation.ViewModel;

namespace _4cube.Presentation.Window
{
    /// <summary>
    /// Interaction logic for GridResizingWindow.xaml
    /// </summary>
    public partial class GridResizingWindow : System.Windows.Window
    {
        private GridResizingViewModel _gridResizingViewModel;
        private IGridModel _gridModel;
        public GridResizingWindow(IGridModel gridModel, GridResizingViewModel gridResizingViewModel)
        {
            InitializeComponent();
            this._gridModel = gridModel;
            DataContext = gridResizingViewModel;
            _gridResizingViewModel = gridResizingViewModel;
            gridResizingViewModel.PropertyChanged+= GridResizingViewModelOnPropertyChanged;
        }

        private void GridResizingViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _gridModel.ResizeGrid(_gridResizingViewModel.Width,_gridResizingViewModel.Height);
        }

        
    }
}
