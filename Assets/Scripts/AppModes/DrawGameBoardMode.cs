using System;
using Interfaces;
using UnityEngine;

namespace AppModes
{
    public class DrawGameBoardMode : IAppMode
    {
        private readonly IGameBoard _gameBoard;
        private readonly IGameCanvas _gameCanvas;
        private readonly IInputSystem _inputSystem;

        private bool _isDrawMode;
        private GridPosition _previousSlotPosition;

        public event EventHandler Finished;

        public DrawGameBoardMode(IAppContext appContext)
        {
            _gameBoard = appContext.Resolve<IGameBoard>();
            _gameCanvas = appContext.Resolve<IGameCanvas>();
            _inputSystem = appContext.Resolve<IInputSystem>();
        }

        public void Activate()
        {
            _gameBoard.CreateGridSlots();

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _gameCanvas.StartGameClick += OnStartGameClick;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
            _gameCanvas.StartGameClick -= OnStartGameClick;
        }

        private void OnPointerDown(object sender, Vector2 mouseWorldPosition)
        {
            if (_gameBoard.IsPointerOnGrid(mouseWorldPosition, out _previousSlotPosition))
            {
                _isDrawMode = true;
                SwitchGridSlotState(_previousSlotPosition);
            }
        }

        private void OnPointerDrag(object sender, Vector2 mouseWorldPosition)
        {
            if (_isDrawMode == false)
            {
                return;
            }

            if (_gameBoard.IsPointerOnGrid(mouseWorldPosition, out var slotPosition) == false)
            {
                return;
            }

            if (IsSameSlot(slotPosition))
            {
                return;
            }

            _previousSlotPosition = slotPosition;
            SwitchGridSlotState(slotPosition);
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _previousSlotPosition.Equals(slotPosition);
        }

        private void SwitchGridSlotState(GridPosition slotPosition)
        {
            if (_gameBoard.IsSlotActive(slotPosition))
            {
                _gameBoard.DeactivateSlot(slotPosition);
            }
            else
            {
                _gameBoard.ActivateSlot(slotPosition);
            }
        }
    }
}