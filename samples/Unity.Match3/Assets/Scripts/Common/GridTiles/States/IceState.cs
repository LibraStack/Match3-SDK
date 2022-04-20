using Common.Enums;

namespace Common.GridTiles.States
{
    public class IceState : StatefulGridTile
    {
        private bool _isLocked = true;

        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;
        public override TileGroup Group => TileGroup.Ice;

        protected override void OnComplete()
        {
            _isLocked = false;
        }

        protected override void OnReset()
        {
            _isLocked = true;
        }
    }
}