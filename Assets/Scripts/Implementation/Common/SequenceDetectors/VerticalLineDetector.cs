using Implementation.Common.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Implementation.Common.SequenceDetectors
{
    public class VerticalLineDetector : LineDetector
    {
        private readonly GridPosition[] _lineDirections;

        public VerticalLineDetector()
        {
            _lineDirections = new[] { GridPosition.Up, GridPosition.Down };
        }

        public override ItemSequence<IUnityItem> GetSequence(IGameBoard<IUnityItem> gameBoard,
            GridPosition gridPosition)
        {
            return GetSequenceByDirection(gameBoard, gridPosition, _lineDirections);
        }
    }
}