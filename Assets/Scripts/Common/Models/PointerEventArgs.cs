using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.Models
{
    public class PointerEventArgs : System.EventArgs
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