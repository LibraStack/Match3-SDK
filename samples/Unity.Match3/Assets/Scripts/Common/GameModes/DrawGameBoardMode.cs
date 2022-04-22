using System;
using Common.Interfaces;
using Common.Models;
using Match3.Core.Structs;
using Match3.Infrastructure.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.GameModes
{
    public class DrawGameBoardMode : IGameMode, IDeactivatable
    {
        private readonly IInputSystem _inputSystem;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        private bool _isDrawMode;
        private bool _isInitialized;
        private GridPosition _previousSlotPosition;

        public DrawGameBoardMode(IAppContext appContext)
        {
            _inputSystem = appContext.Resolve<IInputSystem>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _gameBoardRenderer = appContext.Resolve<IUnityGameBoardRenderer>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            if (_isInitialized == false)
            {
                _isInitialized = true;
                _gameBoardRenderer.CreateGridTiles(null);
            }

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _inputSystem.PointerUp += OnPointerUp;
            _gameUiCanvas.StartGameClick += OnStartGameClick;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
            _inputSystem.PointerUp -= OnPointerUp;
            _gameUiCanvas.StartGameClick -= OnStartGameClick;
        }

        private void OnPointerDown(object sender, PointerEventArgs pointer)
        {
            if (IsPointerOnGrid(pointer.WorldPosition, out var gridPosition) == false)
            {
                return;
            }

            if (IsLeftButton(pointer))
            {
                _isDrawMode = true;
                _previousSlotPosition = gridPosition;

                InvertGridTileState(gridPosition);
            }
            else if (IsRightButton(pointer))
            {
                SetNextGridTileGroup(gridPosition);
            }
        }

        private void OnPointerDrag(object sender, PointerEventArgs pointer)
        {
            if (_isDrawMode == false)
            {
                return;
            }

            if (IsPointerOnGrid(pointer.WorldPosition, out var slotPosition) == false)
            {
                return;
            }

            if (IsSameSlot(slotPosition))
            {
                return;
            }

            _previousSlotPosition = slotPosition;
            InvertGridTileState(slotPosition);
        }

        private void OnPointerUp(object sender, PointerEventArgs pointer)
        {
            _isDrawMode = false;
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsLeftButton(PointerEventArgs pointer)
        {
            return pointer.Button == PointerEventData.InputButton.Left;
        }

        private bool IsRightButton(PointerEventArgs pointer)
        {
            return pointer.Button == PointerEventData.InputButton.Right;
        }

        private bool IsPointerOnGrid(Vector3 worldPosition, out GridPosition gridPosition)
        {
            return _gameBoardRenderer.IsPointerOnGrid(worldPosition, out gridPosition);
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _previousSlotPosition.Equals(slotPosition);
        }

        private void InvertGridTileState(GridPosition gridPosition)
        {
            if (_gameBoardRenderer.IsTileActive(gridPosition))
            {
                _gameBoardRenderer.DeactivateTile(gridPosition);
            }
            else
            {
                _gameBoardRenderer.ActivateTile(gridPosition);
            }
        }

        private void SetNextGridTileGroup(GridPosition gridPosition)
        {
            if (_gameBoardRenderer.IsTileActive(gridPosition))
            {
                _gameBoardRenderer.SetNextGridTileGroup(gridPosition);
            }
        }
    }
}