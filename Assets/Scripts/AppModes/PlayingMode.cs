using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace AppModes
{
    public class PlayingMode : AppMode<EventArgs>
    {
        private readonly IGameBoard _gameBoard;
        private readonly IGameCanvas _gameCanvas;
        private readonly IInputSystem _inputSystem;

        private bool _isDragMode;
        private GridPosition _slotDownPosition;

        public PlayingMode(IAppContext appContext)
        {
            _gameBoard = appContext.Resolve<IGameBoard>();
            _gameCanvas = appContext.Resolve<IGameCanvas>();
            _inputSystem = appContext.Resolve<IInputSystem>();
        }

        public void Configure(int[,] gameBoardData)
        {
            _gameBoard.Create(gameBoardData);
        }

        public override void Activate()
        {
            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _gameCanvas.StartGameClick += OnStartGameClick;
        }

        public override void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
            _gameCanvas.StartGameClick -= OnStartGameClick;
        }

        private void OnPointerDown(object sender, Vector2 mouseWorldPosition)
        {
            if (_gameBoard.IsFilled &&
                _gameBoard.IsPositionOnBoard(mouseWorldPosition, out _slotDownPosition))
            {
                _isDragMode = true;
            }
        }

        private void OnPointerDrag(object sender, Vector2 mouseWorldPosition)
        {
            if (_isDragMode == false)
            {
                return;
            }

            if (_gameBoard.IsPositionOnBoard(mouseWorldPosition, out var slotPosition) == false)
            {
                _isDragMode = false;

                return;
            }

            if (IsSameSlot(slotPosition) || IsDiagonalSlot(slotPosition))
            {
                return;
            }

            _isDragMode = false;
            _gameBoard.SwapItemsAsync(_gameCanvas.GetSelectedFillStrategy(), _slotDownPosition, slotPosition).Forget();
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            _gameBoard.FillAsync(_gameCanvas.GetSelectedFillStrategy()).Forget();
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _slotDownPosition.Equals(slotPosition);
        }

        private bool IsDiagonalSlot(GridPosition slotPosition)
        {
            var isSideSlot = slotPosition.Equals(_slotDownPosition + GridPosition.Up) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Down) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Left) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Right);

            return isSideSlot == false;
        }
    }
}