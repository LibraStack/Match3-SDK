using Terminal.Match3.Enums;

namespace Terminal.Match3.GridTiles.States
{
    public class AvailableState : GridTile
    {
        public override int GroupId => (int) TileGroup.Available;
        public override bool IsLocked => false;
        public override bool CanContainItem => true;
    }
}