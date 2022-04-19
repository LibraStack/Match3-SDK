using System;
using Common.Enums;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IGridTile : IGridSlotState, IDisposable
    {
        TileGroup Group { get; }

        void SetActive(bool value);
        void SetWorldPosition(Vector3 worldPosition);
    }
}