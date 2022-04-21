using Common.Enums;

namespace Common.GridTiles.States
{
    public class NotAvailableState : GridTile
    {
        public override int GroupId => (int) TileGroup.Unavailable;
        public override bool IsLocked => true;
        public override bool CanContainItem => false;
    }
}