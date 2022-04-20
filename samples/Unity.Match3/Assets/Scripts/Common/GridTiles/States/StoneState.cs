using Common.Enums;

namespace Common.GridTiles.States
{
    public class StoneState : StatefulGridTile
    {
        private bool _isLocked = true;
        private bool _canContainItem;

        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => _canContainItem;
        public override TileGroup Group => TileGroup.Stone;

        protected override void OnComplete()
        {
            _isLocked = false;
            _canContainItem = true;
        }

        protected override void OnReset()
        {
            _isLocked = true;
            _canContainItem = false;
        }
    }
}