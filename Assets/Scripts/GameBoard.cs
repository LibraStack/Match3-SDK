using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enums;
using Interfaces;
using Models;
using Solvers;
using UnityEngine;

public class GameBoard : MonoBehaviour, IGameBoard
{
    [SerializeField] private Transform _board;
    [SerializeField] private float _tileSize = 0.6f;

    [SerializeField] private GameObject _tilePrefab;
    
    private int _rowCount;
    private int _columnCount;
    
    private GridSlot[,] _gridSlots;
    private Vector3 _originPosition;

    private IItemSwapper _itemSwapper;
    private IJobsExecutor _jobsExecutor;
    private IGameBoardSolver _gameBoardSolver;

    public bool IsFilled { get; private set; }

    public int RowCount => _rowCount;
    public int ColumnCount => _columnCount;

    public GridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];
    
    public void Create(int[,] gameBoardData)
    {
        _rowCount = gameBoardData.GetLength(0);
        _columnCount = gameBoardData.GetLength(1);
        
        _gridSlots = new GridSlot[_rowCount, _columnCount];
        _originPosition = GetOriginPosition(_rowCount);

        _itemSwapper = new AnimatedItemSwapper(); // TODO: Inject?
        _jobsExecutor = new JobsExecutor(); // TODO: Inject?
        _gameBoardSolver = new LinearGameBoardSolver(this); // TODO: Inject?
        
        for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
            {
                CreateCell(rowIndex, columnIndex, gameBoardData[rowIndex, columnIndex]);
            }
        }
    }

    public async UniTask FillAsync(IBoardFillStrategy fillStrategy)
    {
        await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetFillJobs());
        
        IsFilled = true;
    }

    public async UniTask SwapItemsAsync(IBoardFillStrategy fillStrategy, GridPosition position1, GridPosition position2)
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
        
        _gridSlots[position1.RowIndex, position1.ColumnIndex].Item = item2;
        _gridSlots[position2.RowIndex, position2.ColumnIndex].Item = item1;
    }

    private bool IsSolved(GridPosition position1, GridPosition position2, out IReadOnlyCollection<ItemSequence> sequences)
    {
        sequences = _gameBoardSolver.Solve(this, position1, position2);
        return sequences.Count > 0;
    }

    public bool IsPositionOnBoard(GridPosition gridPosition)
    {
        var isInsideBoard = gridPosition.RowIndex >= 0 &&
                            gridPosition.RowIndex < _rowCount &&
                            gridPosition.ColumnIndex >= 0 &&
                            gridPosition.ColumnIndex < _columnCount;

        if (isInsideBoard == false)
        {
            return false;
        }

        return _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].State != GridSlotState.NotAvailable;
    }

    public bool IsPositionOnBoard(Vector3 worldPosition, out GridPosition gridPosition)
    {
        gridPosition = GetGridPosition(worldPosition);
        return IsPositionOnBoard(gridPosition);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        var rowIndex = (worldPosition - _originPosition).y / _tileSize;
        var columnIndex = (worldPosition - _originPosition).x / _tileSize;

        return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
    }

    public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
    {
        return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
    }

    private Vector3 GetOriginPosition(int dimension)
    {
        var offset = Mathf.Floor(dimension / 2.0f) * _tileSize;
        return new Vector3(-offset, offset);
    }

    private void CreateCell(int rowIndex, int columnIndex, int slotValue)
    {
        var state = slotValue == 1 ? GridSlotState.Free : GridSlotState.NotAvailable;
        _gridSlots[rowIndex, columnIndex] = new GridSlot(state, new GridPosition(rowIndex, columnIndex),
            GetWorldPosition(rowIndex, columnIndex));

        if (slotValue == 1)
        {
            DrawTile(rowIndex, columnIndex);
        }
    }

    private void DrawTile(int rowIndex, int columnIndex)
    {
        Instantiate(_tilePrefab, _board).transform.position = GetWorldPosition(rowIndex, columnIndex);
    }
}
