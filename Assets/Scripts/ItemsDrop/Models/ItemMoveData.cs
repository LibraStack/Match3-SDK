using Common.Interfaces;
using UnityEngine;

namespace ItemsDrop.Models
{
    public class ItemMoveData
    {
        public ItemMoveData(IUnityItem item, Vector3[] worldPositions)
        {
            Item = item;
            WorldPositions = worldPositions;
        }

        public IUnityItem Item { get; }
        public Vector3[] WorldPositions { get; }
    }
}