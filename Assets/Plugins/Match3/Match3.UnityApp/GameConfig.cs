using Match3.Core.Interfaces;
using Match3.UnityApp.Interfaces;

namespace Match3.UnityApp
{
    public class GameConfig<TGridSlot> where TGridSlot : IGridSlot
    {
        public IItemSwapper<TGridSlot> ItemSwapper { get; set; }
        public IGameBoardSolver<TGridSlot> GameBoardSolver { get; set; }
        public ILevelGoalsProvider<TGridSlot> LevelGoalsProvider { get; set; }
        public IGameBoardDataProvider<TGridSlot> GameBoardDataProvider { get; set; }
        public ISolvedSequencesConsumer<TGridSlot>[] SolvedSequencesConsumers { get; set; }
    }
}