using Common.Enums;
using Match3.Core.Models;

namespace Common.Interfaces
{
    public interface IGameBoardAgreements
    {
        bool CanSetItem(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup);
        bool IsBlocker(TileGroup tileGroup);
        bool IsLockedSlot(TileGroup tileGroup);
        bool IsMovableSlot(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup);
        bool IsAvailableSlot(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup);
    }
}