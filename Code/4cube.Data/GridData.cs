using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4cube.Common;
using System.Xml;
using System.IO;
using _4cube.Common.Components;

namespace _4cube.Data
{
    public class GridData : IGridData
    {
        public GridEntity OpenFile(string path)
        {
            TextReader reader = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GridEntity));
                reader = new StreamReader(path);
                return (GridEntity)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public void SaveFile(string path, GridEntity grid)
        {
            TextWriter writer = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GridEntity));
                writer = new StreamWriter(path);
                serializer.Serialize(writer, grid);
            }
            finally
            {
                if (writer != null)
                    writer.Close();

            }
        }
    }
}
