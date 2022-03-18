using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Implementation.Common.Interfaces;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace Implementation.Common
{
    public class GameBoard : MonoBehaviour, IGameBoard, IDisposable
    {
        [SerializeField] private Transform _board;
        [SerializeField] private int _rowCount = 9;
        [SerializeField] private int _columnCount = 9;

        [Space] [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private float _tileSize = 0.6f;

        private GridSlot[,] _gridSlots;
        private Vector3 _originPosition;

        private IItemSwapper _itemSwapper;
        private IJobsExecutor _jobsExecutor;
        private IGameBoardSolver _gameBoardSolver;

        private GameObject[] _gridSlotTiles;

        public bool IsFilled { get; private set; }

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public GridSlot this[Vector3 worldPosition] => this[GetGridPositionByPointer(worldPosition)];
        public GridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public GridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public void Init(IAppContext appContext)
        {
            _itemSwapper = appContext.Resolve<IItemSwapper>();
            _jobsExecutor = appContext.Resolve<IJobsExecutor>();
            _gameBoardSolver = appContext.Resolve<IGameBoardSolver>();
        }

        public void CreateGridSlots()
        {
            _gridSlots = new GridSlot[_rowCount, _columnCount];
            _gridSlotTiles = new GameObject[_rowCount * _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    CreateGridSlot(rowIndex, columnIndex);
                }
            }
        }

        public bool IsSlotActive(GridPosition slotPosition)
        {
            return _gridSlots[slotPosition.RowIndex, slotPosition.ColumnIndex].State == GridSlotState.Free;
        }

        public void ActivateSlot(GridPosition slotPosition)
        {
            _gridSlots[slotPosition.RowIndex, slotPosition.ColumnIndex].Unlock();
            _gridSlotTiles[GetGridSlotTileIndex(slotPosition)].SetActive(true);
        }

        public void DeactivateSlot(GridPosition slotPosition)
        {
            _gridSlots[slotPosition.RowIndex, slotPosition.ColumnIndex].Lock();
            _gridSlotTiles[GetGridSlotTileIndex(slotPosition)].SetActive(false);
        }

        public async UniTask FillAsync(IBoardFillStrategy fillStrategy)
        {
            await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetFillJobs());

            IsFilled = true;
        }

        public async UniTask SwapItemsAsync(IBoardFillStrategy fillStrategy, GridPosition position1,
            GridPosition position2)
        {
            await SwapItems(position1, position2);

            if (IsSolved(position1, position2, out var sequences))
            {
                await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetSolveJobs(sequences));
            }
            else
            {
                await SwapItems(position1, position2);
            }
        }

        private async UniTask SwapItems(GridPosition position1, GridPosition position2)
        {
            var item1 = _gridSlots[position1.RowIndex, position1.ColumnIndex].Item;
            var item2 = _gridSlots[position2.RowIndex, position2.ColumnIndex].Item;

            await _itemSwapper.SwapItemsAsync(item1, item2);

            _gridSlots[position1.RowIndex, position1.ColumnIndex].SetItem(item2);
            _gridSlots[position2.RowIndex, position2.ColumnIndex].SetItem(item1);
        }

        private bool IsSolved(GridPosition position1, GridPosition position2,
            out IReadOnlyCollection<ItemSequence> sequences)
        {
            sequences = _gameBoardSolver.Solve(this, position1, position2);
            return sequences.Count > 0;
        }

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return gridPosition.RowIndex >= 0 &&
                   gridPosition.RowIndex < _rowCount &&
                   gridPosition.ColumnIndex >= 0 &&
                   gridPosition.ColumnIndex < _columnCount;
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            if (IsPositionOnGrid(gridPosition) == false)
            {
                return false;
            }

            return _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].State != GridSlotState.NotAvailable;
        }

        public bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnGrid(gridPosition);
        }

        public bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnBoard(gridPosition);
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return GetWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
        }

        public void Dispose()
        {
            foreach (var gridSlotTile in _gridSlotTiles)
            {
                Destroy(gridSlotTile);
            }

            Array.Clear(_gridSlots, 0, _gridSlots.Length);
            Array.Clear(_gridSlotTiles, 0, _gridSlotTiles.Length);
        }

        private GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - _originPosition).y / _tileSize;
            var columnIndex = (worldPointerPosition - _originPosition).x / _tileSize;

            return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

            return new Vector3(-offsetX, offsetY);
        }

        private void CreateGridSlot(int rowIndex, int columnIndex)
        {
            var index = GetGridSlotTileIndex(rowIndex, columnIndex);
            var gridSlotTile = Instantiate(_tilePrefab, _board);
            gridSlotTile.transform.position = GetWorldPosition(rowIndex, columnIndex);

            _gridSlotTiles[index] = gridSlotTile;
            _gridSlots[rowIndex, columnIndex] =
                new GridSlot(GridSlotState.Free, new GridPosition(rowIndex, columnIndex));
        }

        private int GetGridSlotTileIndex(GridPosition gridPosition)
        {
            return GetGridSlotTileIndex(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        private int GetGridSlotTileIndex(int rowIndex, int columnIndex)
        {
            return rowIndex * _columnCount + columnIndex;
        }
    }
}