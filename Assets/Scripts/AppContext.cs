using System;
using System.Collections.Generic;
using Implementation.Common;
using Implementation.Common.GameBoardSolvers;
using Implementation.Common.Interfaces;
using Implementation.ItemsDrop;
using Implementation.ItemsRollDown;
using Implementation.ItemsScale;
using Match3.Core;
using Match3.Core.Interfaces;
using UnityEngine;

public class AppContext : MonoBehaviour, IAppContext, IDisposable
{
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private GameCanvas _gameCanvas;
    [SerializeField] private ItemGenerator _itemGenerator;
    [SerializeField] private CanvasInputSystem _inputSystem;

    private Dictionary<Type, object> _registeredTypes;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>
        {
            {typeof(IGameBoard), _gameBoard},
            {typeof(IGameCanvas), _gameCanvas},
            {typeof(IInputSystem), _inputSystem},
            {typeof(IItemGenerator), _itemGenerator},
            {typeof(IJobsExecutor), new JobsExecutor()},
            {typeof(IItemSwapper), new AnimatedItemSwapper()},
            {typeof(IGameBoardSolver), new LinearGameBoardSolver()},
            {typeof(IBoardFillStrategy[]), GetBoardFillStrategies(_gameBoard, _itemGenerator)}
        };
    }

    public void Init()
    {
        _gameBoard.Init(this);
        _itemGenerator.InitPool(GetItemsCapacity());
    }

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    public void Dispose()
    {
        _gameBoard.Dispose();
        _itemGenerator.Dispose();
    }

    private int GetItemsCapacity()
    {
        return _gameBoard.RowCount * _gameBoard.ColumnCount +
               Mathf.Max(_gameBoard.RowCount, _gameBoard.ColumnCount) * 2;
    }

    private IBoardFillStrategy[] GetBoardFillStrategies(IGameBoard gameBoard, IItemGenerator itemGenerator)
    {
        return new IBoardFillStrategy[]
        {
            new ItemsScaleFillStrategy(gameBoard, itemGenerator),
            new ItemsDropFillStrategy(gameBoard, itemGenerator),
            new ItemsRollDownFillStrategy(gameBoard, itemGenerator)
        };
    }
}