using System;
using Match3.App.Interfaces;
using Match3.Template.Interfaces;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.GameModes
{
    public class GameInitMode : IGameMode, IDisposable
    {
        private readonly TerminalGame _terminalGame;
        private readonly IItemGenerator _itemGenerator;
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly IBoardFillStrategy<ITerminalGridSlot> _fillStrategy;

        public GameInitMode(TerminalGame terminalGame, IItemGenerator itemGenerator, IGameBoardRenderer gameBoardRenderer,
            IBoardFillStrategy<ITerminalGridSlot> fillStrategy)
        {
            _terminalGame = terminalGame;
            _fillStrategy = fillStrategy;
            _itemGenerator = itemGenerator;
            _gameBoardRenderer = gameBoardRenderer;
        }

        public event EventHandler Finished;

        public void Activate()
        {
            var gameBoardData = new[,]
            {
                { 1, 0, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 1, 1, 1, 2, 1, 1, 1, 1 },
                { 1, 1, 1, 2, 2, 2, 1, 1, 1 },
                { 1, 1, 1, 1, 2, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Math.Max(rowCount, columnCount) * 2;

            _gameBoardRenderer.CreateGridTiles(gameBoardData);
            _itemGenerator.CreateItems(itemsPoolCapacity);

            _terminalGame.SetGameBoardFillStrategy(_fillStrategy);
            _terminalGame.InitGameLevel(0);

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _terminalGame.Dispose();
            _itemGenerator.Dispose();
            _gameBoardRenderer.Dispose();
        }
    }
}