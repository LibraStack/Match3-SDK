using Common.Enums;

namespace Common.GridTiles.States
{
    public class IceState : StatefulGridTile
    {
        private bool _isLocked = true;

        public override bool IsLocked => _isLocked;
        public override bool CanContainItem => true;
        public override TileGroup Group => TileGroup.Ice;

        public override bool NextState()
        {
            var hasNextState = base.NextState();
            if (hasNextState)
            {
                return true;
            }

            _isLocked = false;
            return false;
        }

        public override void ResetState()
        {
            base.ResetState();
            _isLocked = true;
        }
    }
}