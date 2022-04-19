using System;
using Cysharp.Threading.Tasks;
using Match3.App;
using Match3.Core.Structs;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalGame : Match3Game<ITerminalGridSlot>
    {
        private readonly ITerminalInputSystem _inputSystem;
        private readonly ITerminalGameBoardRenderer _gameBoardRenderer;

        private bool _hasSelectedItem;

        public TerminalGame(ITerminalInputSystem inputSystem, ITerminalGameBoardRenderer gameBoardRenderer,
            GameConfig<ITerminalGridSlot> config) : base(config)
        {
            _inputSystem = inputSystem;
            _gameBoardRenderer = gameBoardRenderer;
        }

        protected override void OnGameStarted()
        {
            _inputSystem.KeyPressed += OnKeyPressed;
        }

        protected override void OnGameStopped()
        {
            _inputSystem.KeyPressed -= OnKeyPressed;
        }

        private void OnKeyPressed(object sender, ConsoleKey key)
        {
            if (IsSwapItemsCompleted == false)
            {
                return;
            }

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveCursor(GridPosition.Up);
                    break;
                case ConsoleKey.DownArrow:
                    MoveCursor(GridPosition.Down);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(GridPosition.Left);
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(GridPosition.Right);
                    break;
                case ConsoleKey.Spacebar:
                    SelectItem();
                    break;
            }
        }

        private void MoveCursor(GridPosition direction)
        {
            var gridPosition = _gameBoardRenderer.ActiveGridPosition + direction;

            if (_gameBoardRenderer.IsPositionOnBoard(gridPosition) == false)
            {
                return;
            }

            if (_hasSelectedItem)
            {
                DragItem(gridPosition);
            }

            _gameBoardRenderer.ActivateItem(gridPosition);
        }

        private void SelectItem()
        {
            _hasSelectedItem = true;
            _gameBoardRenderer.SelectActiveGridSlot();
        }

        private void DragItem(GridPosition gridPosition)
        {
            _hasSelectedItem = false;
            _gameBoardRenderer.ClearSelection();

            SwapItemsAsync(_gameBoardRenderer.ActiveGridPosition, gridPosition).Forget();
        }
    }
}