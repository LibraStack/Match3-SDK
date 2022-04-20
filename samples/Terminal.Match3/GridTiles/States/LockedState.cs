using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.GridTiles.States
{
    public class LockedState : GridTile, IStatefulSlot
    {
        private bool _isLocked = true;
        private TileGroup _group = TileGroup.Locked;

        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;
        public override TileGroup Group => _group;

        public bool NextState()
        {
            _isLocked = false;
            _group = TileGroup.Available;

            return false;
        }
    }
}