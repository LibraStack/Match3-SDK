using System.Collections.Generic;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ISpecialItemDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        IEnumerable<TGridSlot> GetSpecialItemGridSlots(IGameBoard<TGridSlot> gameBoard, TGridSlot gridSlot);
    }
}