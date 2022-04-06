using System;
using Common.Enums;
using Common.Extensions;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Helpers;
using Match3.Core.Structs;
using UnityEngine;

namespace Common
{
    public class UnityGameBoardRenderer : MonoBehaviour, IUnityGameBoardRenderer, IGameBoardDataProvider
    {
        [SerializeField] private Transform _board;
        [SerializeField] private int _rowCount = 9;
        [SerializeField] private int _columnCount = 9;

        [Space]
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private float _tileSize = 0.6f;

        private bool[,] _gameBoardData;

        private Vector3 _originPosition;
        private IGridTile[] _gridSlotTiles;

        public bool[,] GetGameBoardData(int level)
        {
            return _gameBoardData;
        }

        public void CreateGridTiles()
        {
            _gameBoardData = new bool[_rowCount, _columnCount];
            _gridSlotTiles = new IGridTile[_rowCount * _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    CreateGridSlotTile(rowIndex, columnIndex);
                }
            }
        }

        public bool IsTileActive(GridPosition gridPosition)
        {
            return _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex];
        }

        public void ActivateTile(GridPosition gridPosition)
        {
            _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex] = true;
            _gridSlotTiles[GetGridSlotTileIndex(gridPosition)].SetGroup(TileGroup.Available);
        }

        public void DeactivateTile(GridPosition gridPosition)
        {
            _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex] = false;
            _gridSlotTiles[GetGridSlotTileIndex(gridPosition)].SetGroup(TileGroup.Unavailable);
        }

        public void ResetState()
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    ResetGridSlotTile(rowIndex, columnIndex);
                }
            }
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
                gridSlotTile.Dispose();
            }

            Array.Clear(_gridSlotTiles, 0, _gridSlotTiles.Length);
            Array.Clear(_gameBoardData, 0, _gameBoardData.Length);
            
            _gridSlotTiles = null;
            _gameBoardData = null;
        }

        private bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(gridPosition, _rowCount, _columnCount);
        }

        private bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionOnGrid(gridPosition) && IsTileActive(gridPosition);
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

        private void CreateGridSlotTile(int rowIndex, int columnIndex)
        {
            var index = GetGridSlotTileIndex(rowIndex, columnIndex);
            var tilePosition = GetWorldPosition(rowIndex, columnIndex);
            var gridSlotTile = _tilePrefab.CreateNew<IGridTile>(tilePosition, _board);

            _gridSlotTiles[index] = gridSlotTile;
            _gameBoardData[rowIndex, columnIndex] = true;
        }

        private void ResetGridSlotTile(int rowIndex, int columnIndex)
        {
            var index = GetGridSlotTileIndex(rowIndex, columnIndex);

            _gameBoardData[rowIndex, columnIndex] = true;
            _gridSlotTiles[index].SetGroup(TileGroup.Available);
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