using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IGameBoardDataProvider
    {
        IGridSlotState[,] GetGameBoardData(int level);
    }
}