using System;
using System.ComponentModel;
using System.Windows;
using _4cube.Bussiness;
using _4cube.Presentation.ViewModel;

namespace _4cube.Presentation.Window
{
    /// <summary>
    /// Interaction logic for GridResizingWindow.xaml
    /// </summary>
    public partial class GridResizingWindow : System.Windows.Window
    {
        private readonly GridResizingViewModel _gridResizingViewModel;
        private readonly IGridModel _gridModel;
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
            try
            {
                _gridModel.ResizeGrid(_gridResizingViewModel.Width, _gridResizingViewModel.Height);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void BtnResize_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
