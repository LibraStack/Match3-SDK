using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Match3.Core;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using Match3.UnityApp.Interfaces;
using Match3.UnityApp.Internal.Interfaces;

namespace Match3.UnityApp.Internal
{
    internal class GameBoard<TGridSlot> : IGameBoard<TGridSlot>, IDisposable where TGridSlot : IGridSlot
    {
        private readonly IJobsExecutor _jobsExecutor;
        private readonly IItemSwapper<TGridSlot> _itemSwapper;
        private readonly IGameBoardSolver<TGridSlot> _gameBoardSolver;

        private int _rowCount;
        private int _columnCount;

        private TGridSlot[,] _gridSlots;

        public GameBoard(IItemSwapper<TGridSlot> itemSwapper, IGameBoardSolver<TGridSlot> gameBoardSolver)
        {
            _itemSwapper = itemSwapper;
            _jobsExecutor = new JobsExecutor();
            _gameBoardSolver = gameBoardSolver;
        }

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public TGridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public TGridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public event EventHandler<IReadOnlyCollection<ItemSequence<TGridSlot>>> SequencesSolved;

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            EnsureGridSlotsIsNotNull();

            return GridMath.IsPositionOnGrid(this, gridPosition);
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionOnGrid(gridPosition) &&
                   _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].CanContainItem;
        }

        public void SetGridSlots(TGridSlot[,] gridSlots)
        {
            if (_gridSlots != null)
            {
                throw new InvalidOperationException("Grid slots have already been created.");
            }

            _rowCount = gridSlots.GetLength(0);
            _columnCount = gridSlots.GetLength(1);
            _gridSlots = gridSlots;
        }

        public async UniTask FillAsync(IBoardFillStrategy<TGridSlot> fillStrategy)
        {
            await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetFillJobs(this));
        }

        public async UniTask SwapItemsAsync(IBoardFillStrategy<TGridSlot> fillStrategy, GridPosition position1,
            GridPosition position2)
        {
            await SwapItems(position1, position2);

            if (IsSolved(position1, position2, out var sequences))
            {
                RaiseSequencesSolved(sequences);
                await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetSolveJobs(this, sequences));
            }
            else
            {
                await SwapItems(position1, position2);
            }
        }

        public void ResetState()
        {
            _rowCount = 0;
            _columnCount = 0;
            _gridSlots = null;
        }

        public void Dispose()
        {
            if (_gridSlots == null)
            {
                return;
            }

            Array.Clear(_gridSlots, 0, _gridSlots.Length);
            ResetState();
        }

        private async Task SwapItems(GridPosition position1, GridPosition position2)
        {
            var gridSlot1 = _gridSlots[position1.RowIndex, position1.ColumnIndex];
            var gridSlot2 = _gridSlots[position2.RowIndex, position2.ColumnIndex];

            await _itemSwapper.SwapItemsAsync(gridSlot1, gridSlot2);
        }

        private bool IsSolved(GridPosition position1, GridPosition position2,
            out IReadOnlyCollection<ItemSequence<TGridSlot>> sequences)
        {
            sequences = _gameBoardSolver.Solve(this, position1, position2);
            return sequences.Count > 0;
        }

        private void RaiseSequencesSolved(IReadOnlyCollection<ItemSequence<TGridSlot>> sequences)
        {
            SequencesSolved?.Invoke(this, sequences);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureGridSlotsIsNotNull()
        {
            if (_gridSlots == null)
            {
                throw new InvalidOperationException("Grid slots are not created.");
            }
        }
    }
}