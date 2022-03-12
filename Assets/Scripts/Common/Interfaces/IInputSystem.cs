using System;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<Vector2> PointerDown;
        event EventHandler<Vector2> PointerDrag;
    }
}