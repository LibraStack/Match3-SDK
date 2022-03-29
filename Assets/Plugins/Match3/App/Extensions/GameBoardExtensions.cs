using Match3.App.Interfaces;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.App.Extensions
{
    public static class GameBoardExtensions
    {
        public static bool CanMoveInDirection<TItem>(this IGameBoard<TItem> gameBoard, GridSlot<TItem> gridSlot,
            GridPosition direction, out GridPosition gridPosition) where TItem : IItem
        {
            var bottomGridSlot = gameBoard.GetSideGridSlot(gridSlot, direction);
            if (bottomGridSlot is { State: GridSlotState.Empty } == false)
            {
                gridPosition = GridPosition.Zero;
                return false;
            }

            gridPosition = bottomGridSlot.GridPosition;
            return true;
        }

        public static GridSlot<TItem> GetSideGridSlot<TItem>(this IGameBoard<TItem> gameBoard, GridSlot<TItem> gridSlot,
            GridPosition direction) where TItem : IItem
        {
            var sideGridSlotPosition = gridSlot.GridPosition + direction;

            return gameBoard.IsPositionOnGrid(sideGridSlotPosition)
                ? gameBoard[sideGridSlotPosition]
                : null;
        }
    }
}