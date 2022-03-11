using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Models
{
    public class ItemDropData
    {
        public IItem Item { get; }
        public List<Vector3> Positions { get; }

        public ItemDropData(IItem item, List<Vector3> positions)
        {
            Item = item;
            Positions = positions;
        }
    }
}