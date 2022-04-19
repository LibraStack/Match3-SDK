using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.GridTiles.States
{
    public class LockedState : GridTile, IStatefulSlot
    {
        private bool _isLocked = true;

        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;
        public override TileGroup Group => TileGroup.Locked;

        public void NextState()
        {
            _isLocked = false;
        }
    }
}