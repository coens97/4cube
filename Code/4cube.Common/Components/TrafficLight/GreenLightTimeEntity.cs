﻿using System.ComponentModel;
using PropertyChanged;

namespace _4cube.Common.Components.TrafficLight
{
    [ImplementPropertyChanged]
    public class GreenLightTimeEntity: INotifyPropertyChanged
    {
        public int Duration { get; set; } = 100;
        public TrafficLightGroup TrafficLightGroupSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}