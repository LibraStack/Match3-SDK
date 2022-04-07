using Common.Enums;
using Common.Interfaces;
using Match3.Core.Enums;
using Match3.Core.Models;

namespace Common
{
    public class GameBoardAgreements : IGameBoardAgreements
    {
        public bool CanSetItem(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup)
        {
            var isAvailableTile = tileGroup == TileGroup.Available || tileGroup == TileGroup.Ice;
            return isAvailableTile && gridSlot.State == GridSlotState.Empty;
        }

        public bool IsBlocker(TileGroup tileGroup)
        {
            return tileGroup == TileGroup.Ice;
        }

        public bool IsInteractableSlot(TileGroup tileGroup)
        {
            return tileGroup == TileGroup.Available;
        }

        public bool IsMovableSlot(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup)
        {
            return IsInteractableSlot(tileGroup) && gridSlot.State == GridSlotState.Occupied;
        }

        public bool IsAvailableSlot(GridSlot<IUnityItem> gridSlot, TileGroup tileGroup)
        {
            return IsInteractableSlot(tileGroup) && gridSlot.State != GridSlotState.NotAvailable;
        }
    }
}