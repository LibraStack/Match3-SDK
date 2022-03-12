using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace ItemsDropImplementation.Models
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