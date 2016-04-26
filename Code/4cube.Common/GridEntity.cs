using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Common
{
    class GridEntity
    {
        public ObservableCollection<CarEntity> Cars { get; set; }
        public List<ComponentEntity> Components { get; set; }
        public Double Height { get; set; }
        public ObservableCollection<PedestrianEntity> Pedestrians { get; set; }
        public double Width { get; set; }


          
    }
}
