using System;
using Match3.Core.Interfaces;

namespace Implementation.Common.Interfaces
{
    public interface IGameCanvas
    {
        event EventHandler StartGameClick;

        IBoardFillStrategy GetSelectedFillStrategy();
    }
}