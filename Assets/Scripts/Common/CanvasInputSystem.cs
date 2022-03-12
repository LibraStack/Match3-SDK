using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public class CanvasInputSystem : MonoBehaviour, IInputSystem
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private EventTrigger _eventTrigger;

        public event EventHandler<Vector2> PointerDown;
        public event EventHandler<Vector2> PointerDrag;

        private void Awake()
        {
            var pointerDown = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            pointerDown.callback.AddListener(data => { OnPointerDown((PointerEventData) data); });

            var pointerDrag = new EventTrigger.Entry {eventID = EventTriggerType.Drag};
            pointerDrag.callback.AddListener(data => { OnPointerDrag((PointerEventData) data); });

            _eventTrigger.triggers.Add(pointerDown);
            _eventTrigger.triggers.Add(pointerDrag);
        }

        private void OnPointerDown(PointerEventData e)
        {
            PointerDown?.Invoke(this, GetWorldPosition(e.position));
        }

        private void OnPointerDrag(PointerEventData e)
        {
            PointerDrag?.Invoke(this, GetWorldPosition(e.position));
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}