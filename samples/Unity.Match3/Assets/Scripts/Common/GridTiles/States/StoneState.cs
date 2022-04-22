using Common.Enums;

namespace Common.GridTiles.States
{
    public class StoneState : StatefulGridTile
    {
        private bool _isLocked = true;
        private bool _canContainItem;
        private int _group = (int) TileGroup.Stone;

        public override int GroupId => _group;
        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => _canContainItem;

        protected override void OnComplete()
        {
            _isLocked = false;
            _canContainItem = true;
            _group = (int) TileGroup.Available;
        }

        protected override void OnReset()
        {
            _isLocked = true;
            _canContainItem = false;
            _group = (int) TileGroup.Stone;
        }
    }
}