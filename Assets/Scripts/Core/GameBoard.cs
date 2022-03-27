using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Core.Enums;
using Match3.Core.Helpers;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core
{
    public class GameBoard<TItem> : IGameBoard<TItem> where TItem : IItem
    {
        private readonly IJobsExecutor _jobsExecutor;
        private readonly IItemSwapper<TItem> _itemSwapper;
        private readonly IGameBoardSolver<TItem> _gameBoardSolver;

        private int _rowCount;
        private int _columnCount;

        private GridSlot<TItem>[,] _gridSlots;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public GridSlot<TItem> this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public GridSlot<TItem> this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public event EventHandler<IReadOnlyCollection<ItemSequence<TItem>>> SequencesSolved;

        public GameBoard(IItemSwapper<TItem> itemSwapper, IGameBoardSolver<TItem> gameBoardSolver)
        {
            _itemSwapper = itemSwapper;
            _jobsExecutor = new JobsExecutor();
            _gameBoardSolver = gameBoardSolver;
        }

        public void Init(bool[,] gameBoardData)
        {
            _rowCount = gameBoardData.GetLength(0);
            _columnCount = gameBoardData.GetLength(1);

            _gridSlots = new GridSlot<TItem>[_rowCount, _columnCount];

            CreateGridSlots(gameBoardData);
        }

        public async UniTask FillAsync(IBoardFillStrategy<TItem> fillStrategy)
        {
            await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetFillJobs(this));
        }

        public async UniTask SwapItemsAsync(IBoardFillStrategy<TItem> fillStrategy, GridPosition position1,
            GridPosition position2)
        {
            await SwapItems(position1, position2);

            if (IsSolved(position1, position2, out var sequences))
            {
                RaiseSequencesSolvedAsync(sequences);
                await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetSolveJobs(this, sequences));
            }
            else
            {
                await SwapItems(position1, position2);
            }
        }

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(this, gridPosition);
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            if (IsPositionOnGrid(gridPosition) == false)
            {
                return false;
            }

            return _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].State != GridSlotState.NotAvailable;
        }

        public void Dispose()
        {
            Array.Clear(_gridSlots, 0, _gridSlots.Length);
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
            out IReadOnlyCollection<ItemSequence<TItem>> sequences)
        {
            sequences = _gameBoardSolver.Solve(this, position1, position2);
            return sequences.Count > 0;
        }

        private void CreateGridSlots(bool[,] gameBoardData)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var isTileActive = gameBoardData[rowIndex, columnIndex];

                    _gridSlots[rowIndex, columnIndex] = new GridSlot<TItem>(
                        isTileActive ? GridSlotState.Empty : GridSlotState.NotAvailable,
                        new GridPosition(rowIndex, columnIndex));
                }
            }
        }

        private void RaiseSequencesSolvedAsync(IReadOnlyCollection<ItemSequence<TItem>> sequences)
        {
            SequencesSolved?.Invoke(this, sequences);
        }
    }
}