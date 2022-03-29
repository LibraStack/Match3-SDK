using System;
using UnityEngine;

namespace Match3.App.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<Vector2> PointerDown;
        event EventHandler<Vector2> PointerDrag;
    }
}