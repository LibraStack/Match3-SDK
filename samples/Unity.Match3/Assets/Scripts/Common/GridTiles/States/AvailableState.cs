using Common.Enums;

namespace Common.GridTiles.States
{
    public class AvailableState : SpriteGridTile
    {
        public override int GroupId => (int) TileGroup.Available;
        public override bool IsLocked => false;
        public override bool CanContainItem => true;
    }
}