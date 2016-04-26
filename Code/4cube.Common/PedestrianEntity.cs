using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace _4cube.Common
{
    public class PedestrianEntity
    {
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}