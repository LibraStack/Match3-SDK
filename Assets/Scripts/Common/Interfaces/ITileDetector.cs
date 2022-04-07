using Match3.Core.Models;

namespace Common.Interfaces
{
    public interface ITileDetector
    {
        void CheckGridSlot(GridSlot<IUnityItem> gridSlot);
    }
}