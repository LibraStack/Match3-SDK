using System;
using Match3.App.Models;

namespace Match3.App.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<PointerEventArgs> PointerDown;
        event EventHandler<PointerEventArgs> PointerDrag;
    }
}