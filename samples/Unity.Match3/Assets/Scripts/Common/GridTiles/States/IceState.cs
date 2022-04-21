using Common.Enums;

namespace Common.GridTiles.States
{
    public class IceState : StatefulGridTile
    {
        private bool _isLocked = true;
        private TileGroup _group = TileGroup.Ice;

        public override int GroupId => (int) _group;
        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;

        protected override void OnComplete()
        {
            _isLocked = false;
            _group = TileGroup.Available;
        }

        protected override void OnReset()
        {
            _isLocked = true;
            _group = TileGroup.Ice;
        }
    }
}