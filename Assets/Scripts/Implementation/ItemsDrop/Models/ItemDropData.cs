using System.Collections.Generic;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.ItemsDrop.Models
{
    public class ItemMoveData
    {
        public IItem Item { get; }
        public IEnumerable<Vector3> Positions { get; }

        public ItemMoveData(IItem item, IEnumerable<Vector3> worldPositions)
        {
            Item = item;
            Positions = worldPositions;
        }
    }
}