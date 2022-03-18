using System;

namespace Match3.Core.Interfaces
{
    public interface IGameCanvas
    {
        event EventHandler StartGameClick;

        IBoardFillStrategy GetSelectedFillStrategy();
    }
}