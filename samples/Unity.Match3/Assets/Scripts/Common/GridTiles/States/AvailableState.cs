using Common.Enums;

namespace Common.GridTiles.States
{
    public class AvailableState : SpriteGridTile
    {
        public override bool IsLocked => false;
        public override bool CanContainItem => true;
        public override TileGroup Group => TileGroup.Available;
    }
}