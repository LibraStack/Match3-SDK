using Match3.Core.Structs;
using Match3.Template.Interfaces;
using Terminal.Match3.Enums;

namespace Terminal.Match3.Interfaces
{
    public interface ITerminalGameBoardRenderer : IGameBoardRenderer
    {
        GridPosition ActiveGridPosition { get; }

        void ActivateItem(GridPosition gridPosition);
        void SelectActiveGridSlot();
        void ClearSelection();
        void RedrawGameBoard();

        bool IsPositionOnBoard(GridPosition gridPosition);
        TileGroup GetTileGroup(GridPosition gridPosition);
        bool TrySetNextTileState(GridPosition gridPosition);
    }
}