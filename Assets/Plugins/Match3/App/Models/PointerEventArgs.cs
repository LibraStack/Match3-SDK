using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.App.Models
{
    public class PointerEventArgs : EventArgs
    {
        public PointerEventArgs(PointerEventData.InputButton button, Vector2 screenPosition, Vector3 worldPosition)
        {
            Button = button;
            WorldPosition = worldPosition;
            ScreenPosition = screenPosition;
        }

        public Vector3 WorldPosition { get; }
        public Vector2 ScreenPosition { get; }
        public PointerEventData.InputButton Button { get; }
    }
}