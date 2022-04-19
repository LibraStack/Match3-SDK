using Match3.App;
using Match3.App.Interfaces;
using Terminal.Match3.Interfaces;
using Terminal.Match3.LevelGoals;

namespace Terminal.Match3
{
    public class LevelGoalsProvider : ILevelGoalsProvider<ITerminalGridSlot>
    {
        public LevelGoal<ITerminalGridSlot>[] GetLevelGoals(int level, IGameBoard<ITerminalGridSlot> gameBoard)
        {
            return new LevelGoal<ITerminalGridSlot>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}