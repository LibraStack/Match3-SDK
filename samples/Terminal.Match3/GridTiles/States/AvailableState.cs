using Terminal.Match3.Enums;

namespace Terminal.Match3.GridTiles.States
{
    public class AvailableState : GridTile
    {
        public override bool IsLocked => false;
        public override bool CanContainItem => true;
        public override TileGroup Group => TileGroup.Available;
    }
}