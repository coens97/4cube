﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace _4cube.Common
{
    [ImplementPropertyChanged]
    public class GridContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public GridEntity Grid = new GridEntity();
    }
}
