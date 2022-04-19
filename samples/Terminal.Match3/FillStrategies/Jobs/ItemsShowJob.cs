using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Match3.App;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.FillStrategies.Jobs
{
    public class ItemsShowJob : Job
    {
        private const char Stage1 = '✵';
        private const char Stage2 = '❄';
        private const int StagesDelay = 150;
        private const ConsoleColor StageColor = ConsoleColor.DarkGray;

        private readonly char[] _icons;
        private readonly ConsoleColor[] _colors;
        private readonly IReadOnlyList<ITerminalItem> _items;
        private readonly ITerminalGameBoardRenderer _terminalGameBoardRenderer;

        public ItemsShowJob(ITerminalGameBoardRenderer terminalGameBoardRenderer,
            IReadOnlyList<ITerminalItem> items) : base(0)
        {
            _terminalGameBoardRenderer = terminalGameBoardRenderer;

            _items = items;
            _icons = new char[items.Count];
            _colors = new ConsoleColor[items.Count];
        }

        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            StoreData();

            await SetIcon(Stage1, StageColor, cancellationToken);
            await SetIcon(Stage2, StageColor, cancellationToken);

            RestoreData();
            RedrawGameBoard();
        }

        private void StoreData()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _icons[i] = _items[i].Icon;
                _colors[i] = _items[i].Color;
            }
        }

        private async UniTask SetIcon(char icon, ConsoleColor color, CancellationToken cancellationToken = default)
        {
            foreach (var item in _items)
            {
                item.Icon = icon;
                item.Color = color;
            }

            RedrawGameBoard();
            await Task.Delay(StagesDelay, cancellationToken);
        }

        private void RestoreData()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Icon = _icons[i];
                _items[i].Color = _colors[i];
            }
        }

        private void RedrawGameBoard()
        {
            _terminalGameBoardRenderer.RedrawGameBoard();
        }
    }
}