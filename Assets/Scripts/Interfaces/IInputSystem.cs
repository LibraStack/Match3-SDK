using System;
using UnityEngine;

namespace Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<Vector2> PointerDown;
        event EventHandler<Vector2> PointerDrag;
    }
}