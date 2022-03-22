using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.ItemsDrop.Models
{
    public class ItemMoveData
    {
        public IItem Item { get; }
        public Vector3[] WorldPositions { get; }

        public ItemMoveData(IItem item, Vector3[] worldPositions)
        {
            Item = item;
            WorldPositions = worldPositions;
        }
    }
}