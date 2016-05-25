using System.Collections.Generic;
using PropertyChanged;
using _4cube.Common.Ai;

namespace _4cube.Common.Components
{
    [ImplementPropertyChanged]
    public class GridEntity
    {
        public List<CarEntity> Cars { get; set; }
        public List<ComponentEntity> Components { get; set; }
        public double Height { get; set; }
        public List<PedestrianEntity> Pedestrians { get; set; }
        public double Width { get; set; }
    }
}
