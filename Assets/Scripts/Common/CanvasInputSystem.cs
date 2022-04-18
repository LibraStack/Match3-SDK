using System;
using Common.Interfaces;
using Common.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public class CanvasInputSystem : MonoBehaviour, IInputSystem
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private EventTrigger _eventTrigger;

        public event EventHandler<PointerEventArgs> PointerDown;
        public event EventHandler<PointerEventArgs> PointerDrag;
        public event EventHandler<PointerEventArgs> PointerUp;

        private void Awake()
        {
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener(data => { OnPointerDown((PointerEventData) data); });

            var pointerDrag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            pointerDrag.callback.AddListener(data => { OnPointerDrag((PointerEventData) data); });

            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener(data => { OnPointerUp((PointerEventData) data); });

            _eventTrigger.triggers.Add(pointerDown);
            _eventTrigger.triggers.Add(pointerDrag);
            _eventTrigger.triggers.Add(pointerUp);
        }

        private void OnPointerDown(PointerEventData e)
        {
            PointerDown?.Invoke(this, GetPointerEventArgs(e));
        }

        private void OnPointerDrag(PointerEventData e)
        {
            PointerDrag?.Invoke(this, GetPointerEventArgs(e));
        }

        private void OnPointerUp(PointerEventData e)
        {
            PointerUp?.Invoke(this, GetPointerEventArgs(e));
        }

        private PointerEventArgs GetPointerEventArgs(PointerEventData e)
        {
            return new PointerEventArgs(e.button, GetWorldPosition(e.position));
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}