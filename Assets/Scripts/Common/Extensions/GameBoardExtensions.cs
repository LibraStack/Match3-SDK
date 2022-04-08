using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Common.Extensions
{
    public static class GameBoardExtensions
    {
        public static bool CanMoveDown(this IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            out GridPosition gridPosition)
        {
            var bottomGridSlot = gameBoard.GetSideGridSlot(gridSlot, GridPosition.Down);
            if (bottomGridSlot is { CanSetItem: true })
            {
                gridPosition = bottomGridSlot.GridPosition;
                return true;
            }

            gridPosition = GridPosition.Zero;
            return false;
        }

        public static GridSlot<IUnityItem> GetSideGridSlot(this IGameBoard<IUnityItem> gameBoard,
            GridSlot<IUnityItem> gridSlot, GridPosition direction)
        {
            var sideGridSlotPosition = gridSlot.GridPosition + direction;

            return gameBoard.IsPositionOnGrid(sideGridSlotPosition)
                ? gameBoard[sideGridSlotPosition]
                : null;
        }
    }
}