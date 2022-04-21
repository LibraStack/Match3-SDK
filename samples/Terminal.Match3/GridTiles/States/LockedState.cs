using Terminal.Match3.Enums;
using Match3.Core.Interfaces;

namespace Terminal.Match3.GridTiles.States
{
    public class LockedState : GridTile, IStatefulSlot
    {
        private bool _isLocked = true;
        private TileGroup _group = TileGroup.Locked;

        public override int GroupId => (int) _group;
        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;

        public bool NextState()
        {
            _isLocked = false;
            _group = TileGroup.Available;

            return false;
        }

        public void ResetState()
        {
            _isLocked = true;
            _group = TileGroup.Locked;
        }
    }
}