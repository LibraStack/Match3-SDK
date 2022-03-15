using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace ItemsDropImplementation.Models
{
    public class ItemMoveData
    {
        private readonly List<Vector3> _worldPositions;

        public IItem Item { get; }
        public IReadOnlyList<Vector3> Positions => _worldPositions;

        public ItemMoveData(IItem item, List<Vector3> worldPositions)
        {
            Item = item;
            _worldPositions = worldPositions;
        }

        public void AddPosition(Vector3 worldPosition)
        {
            _worldPositions.Add(worldPosition);
        }
    }
}