using Terminal.Match3.Enums;

namespace Terminal.Match3.GridTiles.States
{
    public class NotAvailableState : GridTile
    {
        public override bool IsLocked => true;
        public override bool CanContainItem => false;
        public override TileGroup Group => TileGroup.Unavailable;
    }
}