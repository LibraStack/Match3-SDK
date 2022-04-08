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

        public override bool NextState()
        {
            var hasNextState = base.NextState();
            if (hasNextState)
            {
                return true;
            }

            _isLocked = false;
            _canContainItem = true;

            return false;
        }

        public override void ResetState()
        {
            base.ResetState();

            _isLocked = true;
            _canContainItem = false;
        }
    }
}