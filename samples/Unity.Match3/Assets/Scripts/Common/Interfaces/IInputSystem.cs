using System;
using Common.Models;

namespace Common.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<PointerEventArgs> PointerDown;
        event EventHandler<PointerEventArgs> PointerDrag;
        event EventHandler<PointerEventArgs> PointerUp;
    }
}