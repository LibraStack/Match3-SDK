using Implementation.Common.Interfaces;
using UnityEngine;

namespace Implementation.ItemsDrop.Models
{
    public class ItemMoveData
    {
        public IUnityItem Item { get; }
        public Vector3[] WorldPositions { get; }

        public ItemMoveData(IUnityItem item, Vector3[] worldPositions)
        {
            Item = item;
            WorldPositions = worldPositions;
        }
    }
}