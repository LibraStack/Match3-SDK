using System;
using Common.Interfaces;
using Common.Structs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.AppModes
{
    public class PlayingMode : IAppMode
    {
        private readonly IGameBoard _gameBoard;
        private readonly IGameCanvas _gameCanvas;
        private readonly IInputSystem _inputSystem;

        private bool _isDragMode;
        private GridPosition _slotDownPosition;

        public event EventHandler Finished;

        public PlayingMode(IAppContext appContext)
        {
            _gameBoard = appContext.Resolve<IGameBoard>();
            _gameCanvas = appContext.Resolve<IGameCanvas>();
            _inputSystem = appContext.Resolve<IInputSystem>();
        }

        public void Activate()
        {
            FillGameBoard();

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
        }

        private void OnPointerDown(object sender, Vector2 mouseWorldPosition)
        {
            if (_gameBoard.IsFilled &&
                _gameBoard.IsPointerOnBoard(mouseWorldPosition, out _slotDownPosition))
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

            if (_gameBoard.IsPointerOnBoard(mouseWorldPosition, out var slotPosition) == false)
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

        private void FillGameBoard()
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