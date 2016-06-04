using _4cube.Common;
using _4cube.Common.Components;

namespace _4cube.Bussiness
{
    public interface IGridModel
    {
        GridEntity Grid { get; }
        void AddComponent(ComponentEntity component);
        void DeleteComponent(ComponentEntity component);
        void OpenFile(string path);
        void ResizeGrid(int w, int h);
        void RotateComponent(ComponentEntity component);
        void SaveFile(string path);
        void New();
    }
}
