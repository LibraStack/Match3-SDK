using System;
using Match3.App.Interfaces;
using Match3.Core;
using Match3.Core.Structs;
using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalGameBoardRenderer : ITerminalGameBoardRenderer, IGameBoardDataProvider<ITerminalGridSlot>
    {
        private const int OffsetX = 5;
        private const int OffsetY = 1;
        private const string IconsSpace = "  ";

        private readonly TileItemsPool _tileItemsPool;

        private int _rowCount;
        private int _columnCount;

        private IGridTile[,] _gridSlotTiles;
        private ITerminalGridSlot[,] _gameBoardSlots;

        private ITerminalGridSlot _activeGridSlot;
        private ITerminalGridSlot _selectedGridSlot;

        public TerminalGameBoardRenderer()
        {
            _tileItemsPool = new TileItemsPool(new[]
            {
                TileGroup.Available,
                TileGroup.Unavailable,
                TileGroup.Locked
            });
        }

        public GridPosition ActiveGridPosition => _activeGridSlot?.GridPosition ?? GridPosition.Zero;

        public void CreateGridTiles(int[,] data)
        {
            _rowCount = data.GetLength(0);
            _columnCount = data.GetLength(1);

            _gridSlotTiles = new IGridTile[_rowCount, _columnCount];
            _gameBoardSlots = new ITerminalGridSlot[_rowCount, _columnCount];

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var tileGroup = (TileGroup) data[rowIndex, columnIndex];
                    var gridTile = _tileItemsPool.GetGridTile(tileGroup);

                    _gridSlotTiles[rowIndex, columnIndex] = gridTile;
                    _gameBoardSlots[rowIndex, columnIndex] =
                        new TerminalGridSlot(gridTile, new GridPosition(rowIndex, columnIndex));
                }
            }

            _activeGridSlot = _gameBoardSlots[0, 0];
        }

        public void ResetGridTiles()
        {
            Dispose();
        }

        public void RedrawGameBoard()
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                Console.SetCursorPosition(OffsetX, OffsetY + rowIndex);

                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var gridSlot = _gameBoardSlots[rowIndex, columnIndex];
                    DrawGridSlot(gridSlot, GetItemColor(gridSlot));
                }

                Console.Write(' ');
            }

            Console.WriteLine();
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(gridPosition, _rowCount, _columnCount) &&
                   _gameBoardSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].IsMovable;
        }

        public void ActivateItem(GridPosition gridPosition)
        {
            _activeGridSlot = _gameBoardSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
            RedrawGameBoard();
        }

        public ITerminalGridSlot[,] GetGameBoardSlots(int level)
        {
            return _gameBoardSlots;
        }

        public void SelectActiveGridSlot()
        {
            _selectedGridSlot = _activeGridSlot;
            RedrawGameBoard();
        }

        public void ClearSelection()
        {
            _selectedGridSlot = null;
            RedrawGameBoard();
        }

        public void Dispose()
        {
            DisposeArray(_gridSlotTiles);
            DisposeArray(_gameBoardSlots);
        }

        private void DrawGridSlot(ITerminalGridSlot gridSlot, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(gridSlot.Item.Icon);
            Console.Write(IconsSpace);
        }

        private ConsoleColor GetItemColor(ITerminalGridSlot gridSlot)
        {
            if (gridSlot.CanContainItem && gridSlot.IsLocked)
            {
                return ConsoleColor.DarkGray;
            }

            if (_selectedGridSlot != null && _selectedGridSlot.GridPosition == gridSlot.GridPosition)
            {
                return ConsoleColor.White;
            }

            return _activeGridSlot.GridPosition == gridSlot.GridPosition ? ConsoleColor.Gray : gridSlot.Item.Color;
        }

        private void DisposeArray(Array array)
        {
            if (array != null)
            {
                Array.Clear(array, 0, array.Length);
            }
        }
    }
}