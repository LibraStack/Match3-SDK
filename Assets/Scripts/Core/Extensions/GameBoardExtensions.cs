using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core.Extensions
{
    public static class GameBoardExtensions
    {
        public static bool CanMoveInDirection(this IGameBoard gameBoard, GridSlot gridSlot, GridPosition direction,
            out GridPosition gridPosition)
        {
            var bottomGridSlot = gameBoard.GetSideGridSlot(gridSlot, direction);
            if (bottomGridSlot is { State: GridSlotState.Free } == false)
            {
                gridPosition = GridPosition.Zero;
                return false;
            }

            gridPosition = bottomGridSlot.GridPosition;
            return true;
        }

        public static GridSlot GetSideGridSlot(this IGameBoard gameBoard, GridSlot gridSlot, GridPosition direction)
        {
            var sideGridSlotPosition = gridSlot.GridPosition + direction;

            return gameBoard.IsPositionOnGrid(sideGridSlotPosition)
                ? gameBoard[sideGridSlotPosition]
                : null;
        }
    }
}