using Match3.Core.Structs;
using Match3.Template.Interfaces;

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
    }
}