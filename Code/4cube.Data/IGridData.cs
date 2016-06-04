using _4cube.Common;


namespace _4cube.Data
{
    public interface IGridData
    {
         void SaveFile(string path, GridEntity grid);

         GridEntity OpenFile(string path);
    }
}
