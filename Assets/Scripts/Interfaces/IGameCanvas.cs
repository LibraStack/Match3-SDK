using System;

namespace Interfaces
{
    public interface IGameCanvas
    {
        event EventHandler StartGameClick;
        
        IBoardFillStrategy GetSelectedFillStrategy();
    }
}