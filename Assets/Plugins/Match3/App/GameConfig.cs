using Match3.App.Interfaces;
using Match3.Core.Interfaces;

namespace Match3.App
{
    public class GameConfig<TItem> where TItem : IItem
    {
        public IInputSystem InputSystem { get; set; }
        public IItemSwapper<TItem> ItemSwapper { get; set; }
        public IGameBoardRenderer GameBoardRenderer { get; set; }
        public IBoardFillStrategy<TItem> FillStrategy { get; set; }
        public IGameBoardSolver<TItem> GameBoardSolver { get; set; }
        public ILevelGoalsProvider<TItem> LevelGoalsProvider { get; set; }
        public IGameBoardDataProvider<TItem> GameBoardDataProvider { get; set; }
        public ISolvedSequencesConsumer<TItem>[] SolvedSequencesConsumers { get; set; }
    }
}