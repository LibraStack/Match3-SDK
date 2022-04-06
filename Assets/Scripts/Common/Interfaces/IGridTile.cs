using System;
using Common.Enums;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IGridTile : IDisposable
    {
        TileGroup Group { get; }

        void SetActive(bool value);
        void SetWorldPosition(Vector3 worldPosition);
    }
}