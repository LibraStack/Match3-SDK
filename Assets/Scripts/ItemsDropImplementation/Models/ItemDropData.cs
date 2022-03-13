using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace ItemsDropImplementation.Models
{
    public class ItemDropData // TODO: Rename to ItemsMoveData
    {
        public IItem Item { get; }
        public HashSet<Vector3> Positions { get; set; }

        public ItemDropData(IItem item, HashSet<Vector3> positions)
        {
            Item = item;
            Positions = positions;
        }
    }
}