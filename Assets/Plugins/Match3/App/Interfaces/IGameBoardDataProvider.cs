using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Match3.App.Interfaces
{
    public interface IGameBoardDataProvider<TItem> where TItem : IItem
    {
        GridSlot<TItem>[,] GetGameBoardSlots(int level);
    }
}