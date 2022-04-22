using Match3.App;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Infrastructure.SequenceDetectors
{
    public class VerticalLineDetector<TGridSlot> : LineDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        private readonly GridPosition[] _lineDirections;

        public VerticalLineDetector()
        {
            _lineDirections = new[] { GridPosition.Up, GridPosition.Down };
        }

        public override ItemSequence<TGridSlot> GetSequence(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition)
        {
            return GetSequenceByDirection(gameBoard, gridPosition, _lineDirections);
        }
    }
}