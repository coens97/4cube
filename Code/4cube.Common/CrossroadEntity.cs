using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4cube.Common
{
    public class CrossroadEntity:ComponentEntity
    {
        public List<GreenLightTimeEntity> GreenLightTimeEntities { get; set; }
    }
}
