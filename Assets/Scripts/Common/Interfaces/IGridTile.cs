using System;
using Common.Enums;

namespace Common.Interfaces
{
    public interface IGridTile : IDisposable
    {
        TileGroup Group { get; }

        void SetGroup(TileGroup group);
    }
}