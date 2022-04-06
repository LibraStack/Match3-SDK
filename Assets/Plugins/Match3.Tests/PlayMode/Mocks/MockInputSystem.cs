using System;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Structs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Tests.PlayMode.Mocks
{
    public class MockInputSystem : IInputSystem
    {
        public event EventHandler<PointerEventArgs> PointerDown;
        public event EventHandler<PointerEventArgs> PointerDrag;
        public event EventHandler<PointerEventArgs> PointerUp;

        public void InvokePointerDown(GridPosition gridPosition)
        {
            PointerDown?.Invoke(this, CreatePointerEventArgs(gridPosition));
        }

        public void InvokePointerDrag(GridPosition gridPosition)
        {
            PointerDrag?.Invoke(this, CreatePointerEventArgs(gridPosition));
        }

        private PointerEventArgs CreatePointerEventArgs(GridPosition gridPosition)
        {
            return new PointerEventArgs(PointerEventData.InputButton.Left, Vector2.zero,
                new Vector3(gridPosition.ColumnIndex, gridPosition.RowIndex));
        }
    }
}