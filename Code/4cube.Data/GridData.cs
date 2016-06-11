using System;
using _4cube.Common;
using System.IO;
using _4cube.Common.Components;
using _4cube.Common.Components.Crossroad;

namespace _4cube.Data
{
    public class GridData : IGridData
    {
        private static readonly Type[] _unkownTypes =
        {typeof(CrossroadAEntity), typeof(CrossroadBEntity), typeof(CurvedRoadEntity), typeof(StraightRoadEntity)};
    public GridEntity OpenFile(string path)
        {
            TextReader reader = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GridEntity), _unkownTypes);
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
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GridEntity), _unkownTypes);
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
