using System;

namespace Implementation.Common.Interfaces
{
    public interface IGameUiCanvas
    {
        int SelectedIconsSetIndex { get; }
        int SelectedFillStrategyIndex { get; }
        
        event EventHandler StartGameClick;
    }
}