using System;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Structs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.AppModes
{
    public class DrawGameBoardMode : IAppMode, IDeactivatable
    {
        private readonly IInputSystem _inputSystem;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        private bool _isDrawMode;
        private bool _isInitialized;
        private GridPosition _previousSlotPosition;

        public event EventHandler Finished;

        public DrawGameBoardMode(IAppContext appContext)
        {
            _inputSystem = appContext.Resolve<IInputSystem>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _gameBoardRenderer = appContext.Resolve<IUnityGameBoardRenderer>();
        }

        public void Activate()
        {
            if (_isInitialized == false)
            {
                _isInitialized = true;
                _gameBoardRenderer.CreateGridTiles();
            }

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _gameUiCanvas.StartGameClick += OnStartGameClick;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
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

        private void OnStartGameClick(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsLeftButton(PointerEventArgs pointer)
        {
            return pointer.Button == PointerEventData.InputButton.Left;
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
    }
}