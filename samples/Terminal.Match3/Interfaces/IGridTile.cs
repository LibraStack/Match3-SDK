using Match3.Core.Interfaces;
using Terminal.Match3.Enums;

namespace Terminal.Match3.Interfaces
{
    public interface IGridTile : IGridSlotState
    {
        TileGroup Group { get; }
    }
}