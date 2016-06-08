using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using _4cube.Bussiness;
using _4cube.Bussiness.Config;
using _4cube.Common;
using _4cube.Presentation.Annotations;

namespace _4cube.Presentation.ViewModel
{
    [ImplementPropertyChanged]
    public class GridResizingViewModel: INotifyPropertyChanged
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public GridResizingViewModel()
        {
        }

        public GridResizingViewModel(IGridModel gridModel)
        {
            Height = gridModel.Grid.Height;
            Width = gridModel.Grid.Width;
        }

    }
}
