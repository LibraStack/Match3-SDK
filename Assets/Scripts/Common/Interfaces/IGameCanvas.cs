using System;

namespace Common.Interfaces
{
    public interface IGameCanvas
    {
        event EventHandler StartGameClick;

        IBoardFillStrategy GetSelectedFillStrategy();
    }
}