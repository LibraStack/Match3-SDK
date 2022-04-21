using System.Collections.Generic;
using Common.Enums;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;
using Match3.Core.Interfaces;
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

        public IGridTile GetGridTile(TileGroup tileGroup)
        {
            var tiles = _itemsPool[tileGroup];
            var gridTile = tiles.Count == 0 ? CreateTile(_tilePrefabs[tileGroup]) : tiles.Dequeue();
            gridTile.SetActive(true);

            return gridTile;
        }

        public void ReturnGridTile(IGridTile gridTile)
        {
            if (gridTile is IStatefulSlot statefulSlot)
            {
                statefulSlot.ResetState();
            }

            gridTile.SetActive(false);
            _itemsPool[(TileGroup) gridTile.GroupId].Enqueue(gridTile);
        }

        private IGridTile CreateTile(GameObject tilePrefab)
        {
            return tilePrefab.CreateNew<IGridTile>(parent: _itemsContainer);
        }
    }
}