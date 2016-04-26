using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Common
{
    public class GridEntity
    {
        public List<CarEntity> Cars { get; set; }
        public List<ComponentEntity> Components { get; set; }
        public double Height { get; set; }
        public List<PedestrianEntity> Pedestrians { get; set; }
        public double Width { get; set; }


          
    }
}
