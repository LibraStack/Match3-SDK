using System;
using Common.Enums;
using UnityEngine;

namespace Common.Models
{
    [Serializable]
    public class TileModel
    {
        [SerializeField] private TileGroup _group;
        [SerializeField] private GameObject _prefab;

        public TileGroup Group => _group;
        public GameObject Prefab => _prefab;
    }
}