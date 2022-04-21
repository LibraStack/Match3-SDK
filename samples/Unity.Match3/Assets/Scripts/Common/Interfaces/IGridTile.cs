using System;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IGridTile : IGridSlotState, IDisposable
    {
        void SetActive(bool value);
        void SetWorldPosition(Vector3 worldPosition);
    }
}