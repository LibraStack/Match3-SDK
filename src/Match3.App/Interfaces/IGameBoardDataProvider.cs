using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IGameBoardDataProvider<out TGridSlot> where TGridSlot : IGridSlot
    {
        TGridSlot[,] GetGameBoardSlots(int level);
    }
}