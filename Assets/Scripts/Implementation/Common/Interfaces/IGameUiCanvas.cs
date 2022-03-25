using System;

namespace Implementation.Common.Interfaces
{
    public interface IGameUiCanvas
    {
        int SelectedFillStrategyIndex { get; }
        
        event EventHandler StartGameClick;
    }
}