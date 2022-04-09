using System.Collections.Generic;
using Common.Enums;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;
using UnityEngine;

namespace Common
{
    public class TileItemsPool
    {
        private readonly Transform _itemsContainer;
        private readonly Dictionary<TileGroup, GameObject> _tilePrefabs;
        private readonly Dictionary<TileGroup, Queue<IGridTile>> _itemsPool;

        public TileItemsPool(IReadOnlyCollection<TileModel> tiles, Transform itemsContainer)
        {
            _itemsContainer = itemsContainer;
            _itemsPool = new Dictionary<TileGroup, Queue<IGridTile>>(tiles.Count);
            _tilePrefabs = new Dictionary<TileGroup, GameObject>(tiles.Count);

            foreach (var tile in tiles)
            {
                _tilePrefabs.Add(tile.Group, tile.Prefab);
                _itemsPool.Add(tile.Group, new Queue<IGridTile>());
            }
        }

        public IGridTile GetTile(TileGroup group)
        {
            var tiles = _itemsPool[group];
            var tile = tiles.Count == 0 ? CreateTile(_tilePrefabs[group]) : tiles.Dequeue();
            tile.SetActive(true);

            return tile;
        }

        public void ReturnTile(IGridTile tile)
        {
            if (tile is IStatefulSlot statefulSlot)
            {
                statefulSlot.ResetState();
            }

            tile.SetActive(false);
            _itemsPool[tile.Group].Enqueue(tile);
        }

        private IGridTile CreateTile(GameObject tilePrefab)
        {
            return tilePrefab.CreateNew<IGridTile>(parent: _itemsContainer);
        }
    }
}