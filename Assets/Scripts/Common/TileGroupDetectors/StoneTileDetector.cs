using Common.Enums;
using Common.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Common.TileGroupDetectors
{
    public class StoneTileDetector : ITileDetector
    {
        private readonly GridPosition[] _lookupDirections;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public StoneTileDetector(IUnityGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
            _lookupDirections = new[]
            {
                GridPosition.Up,
                GridPosition.Down,
                GridPosition.Left,
                GridPosition.Right
            };
        }

        public void CheckGridSlot(GridSlot<IUnityItem> gridSlot)
        {
            foreach (var lookupDirection in _lookupDirections)
            {
                var position = gridSlot.GridPosition + lookupDirection;
                if (_gameBoardRenderer.IsPositionOnGrid(position) &&
                    _gameBoardRenderer.GetTileGroup(position) == TileGroup.Stone)
                {
                    _gameBoardRenderer.TrySetNextTileState(position);
                }
            }
        }
    }
}