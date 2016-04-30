using System.Collections.Generic;
using _4cube.Common.Ai;

namespace _4cube.Common.Components
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
