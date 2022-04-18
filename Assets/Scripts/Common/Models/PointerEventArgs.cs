using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.Models
{
    public class PointerEventArgs : System.EventArgs
    {
        public PointerEventArgs(PointerEventData.InputButton button, Vector3 worldPosition)
        {
            Button = button;
            WorldPosition = worldPosition;
        }

        public Vector3 WorldPosition { get; }
        public PointerEventData.InputButton Button { get; }
    }
}