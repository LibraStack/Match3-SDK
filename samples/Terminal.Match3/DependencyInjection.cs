using Match3.App;
using Match3.App.Interfaces;
using Match3.Template;
using Match3.Template.Interfaces;
using Match3.Template.SequenceDetectors;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Match3.FillStrategies;
using Terminal.Match3.Interfaces;
using Terminal.Match3.SpecialItemDetectors;

namespace Terminal.Match3
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            var gameBoardRenderer = new TerminalGameBoardRenderer();
            services.AddSingleton<IGameBoardRenderer>(gameBoardRenderer);
            services.AddSingleton<ITerminalGameBoardRenderer>(gameBoardRenderer);

            var gameConfig = GetGameConfig(gameBoardRenderer);
            services.AddSingleton<GameConfig<ITerminalGridSlot>>(gameConfig);

            var itemGenerator = new TerminalItemGenerator();
            services.AddSingleton<IItemGenerator>(itemGenerator);
            services.AddSingleton<IItemsPool<ITerminalItem>>(itemGenerator);

            services.AddSingleton<TerminalGame>();
            services.AddSingleton<ITerminalInputSystem, TerminalInputSystem>();
            services.AddSingleton<IBoardFillStrategy<ITerminalGridSlot>, SimpleFillStrategy>();
        }

        private static GameConfig<ITerminalGridSlot> GetGameConfig(TerminalGameBoardRenderer gameBoardRenderer)
        {
            return new GameConfig<ITerminalGridSlot>
            {
                GameBoardDataProvider = gameBoardRenderer,
                LevelGoalsProvider = new LevelGoalsProvider(),
                ItemSwapper = new TerminalItemSwapper(gameBoardRenderer),
                GameBoardSolver = GetGameBoardSolver(),
                SolvedSequencesConsumers = GetSolvedSequencesConsumers()
            };
        }

        private static IGameBoardSolver<ITerminalGridSlot> GetGameBoardSolver()
        {
            return new GameBoardSolver<ITerminalGridSlot>(GetSequenceDetectors(), GetSpecialItemDetectors());
        }

        private static ISequenceDetector<ITerminalGridSlot>[] GetSequenceDetectors()
        {
            return new ISequenceDetector<ITerminalGridSlot>[]
            {
                new VerticalLineDetector<ITerminalGridSlot>(),
                new HorizontalLineDetector<ITerminalGridSlot>()
            };
        }

        private static ISpecialItemDetector<ITerminalGridSlot>[] GetSpecialItemDetectors()
        {
            return new ISpecialItemDetector<ITerminalGridSlot>[]
            {
                new LockedItemDetector()
            };
        }

        private static ISolvedSequencesConsumer<ITerminalGridSlot>[] GetSolvedSequencesConsumers()
        {
            return System.Array.Empty<ISolvedSequencesConsumer<ITerminalGridSlot>>();
        }
    }
}