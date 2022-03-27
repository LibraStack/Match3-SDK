using System;
using Implementation.Common.LevelGoals;

namespace Implementation.Common.Interfaces
{
    public interface IGameUiCanvas
    {
        int SelectedIconsSetIndex { get; }
        int SelectedFillStrategyIndex { get; }

        event EventHandler StartGameClick;

        void ShowMessage(string message);
        void RegisterAchievedGoal(LevelGoal achievedGoal);
    }
}