using System;
using System.Collections.Generic;
using PropertyChanged;
using System.ComponentModel;
using System.Threading;
using _4cube.Common.Ai;

namespace _4cube.Common.Components
{
    [ImplementPropertyChanged]
    public class ComponentEntity : INotifyPropertyChanged
    {
        [DoNotNotify]
        public int[] NrOfIncomingCars { get; set; } = {5, 5, 5, 5};
        [DoNotNotify]
        public int[] NrOfIncomingCarsSpawned { get; set; } = { 0, 0, 0, 0 };
        public Direction Rotation { get; set; } = Direction.Up;
        [DoNotNotify]
        public int X { get; set; }
        [DoNotNotify]
        public int Y { get; set; }
        [DoNotNotify]
        public List<CarEntity> CarsInComponent = new List<CarEntity>();
        [DoNotNotify, NonSerialized]
        public ReaderWriterLockSlim CarsInComponentLock = new ReaderWriterLockSlim();


        public event PropertyChangedEventHandler PropertyChanged;
    }
}