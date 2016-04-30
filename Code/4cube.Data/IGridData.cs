using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using _4cube.Common.Components;


namespace _4cube.Data
{
    public interface IGridData
    {
         void SaveFile(string path, GridEntity grid);

         GridEntity OpenFile(string path);
    }
}
