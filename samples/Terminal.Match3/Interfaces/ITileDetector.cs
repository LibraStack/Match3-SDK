using Match3.Core.Interfaces;

namespace Terminal.Match3.Interfaces
{
    public interface ITileDetector<in TGridSlot> where TGridSlot : IGridSlot
    {
        void CheckGridSlot(TGridSlot gridSlot);
    }
}