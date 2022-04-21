using Terminal.Match3.Interfaces;

namespace Terminal.Match3.GridTiles
{
    public abstract class GridTile : IGridTile
    {
        public abstract int GroupId { get; }
        public abstract bool IsLocked { get; }
        public abstract bool CanContainItem { get; }
    }
}