using Terminal.Match3.Enums;
using Match3.Core.Interfaces;

namespace Terminal.Match3.GridTiles.States
{
    public class LockedState : GridTile, IStatefulSlot
    {
        private bool _isLocked = true;
        private int _group = (int) TileGroup.Locked;

        public override int GroupId => _group;
        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;

        public bool NextState()
        {
            _isLocked = false;
            _group = (int) TileGroup.Available;

            return false;
        }

        public void ResetState()
        {
            _isLocked = true;
            _group = (int) TileGroup.Locked;
        }
    }
}