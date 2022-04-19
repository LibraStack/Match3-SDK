using System;
using System.Collections.Generic;
using Terminal.Match3.Enums;
using Terminal.Match3.GridTiles.States;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TileItemsPool
    {
        private readonly Dictionary<TileGroup, Queue<IGridTile>> _itemsPool;

        public TileItemsPool(IReadOnlyCollection<TileGroup> tileGroups)
        {
            _itemsPool = new Dictionary<TileGroup, Queue<IGridTile>>(tileGroups.Count);

            foreach (var tileGroup in tileGroups)
            {
                _itemsPool.Add(tileGroup, new Queue<IGridTile>());
            }
        }

        public IGridTile GetGridTile(TileGroup tileGroup)
        {
            var tiles = _itemsPool[tileGroup];
            return tiles.Count == 0 ? CreateTile(tileGroup) : tiles.Dequeue();
        }

        private IGridTile CreateTile(TileGroup tileGroup)
        {
            return tileGroup switch
            {
                TileGroup.Unavailable => new NotAvailableState(),
                TileGroup.Available => new AvailableState(),
                TileGroup.Locked => new LockedState(),
                _ => throw new ArgumentOutOfRangeException(nameof(tileGroup), tileGroup, null)
            };
        }
    }
}