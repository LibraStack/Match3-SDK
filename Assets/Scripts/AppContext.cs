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
            {typeof(IGameBoard<IUnityItem>), _gameBoard},
            {typeof(IGameCanvas), _gameCanvas},
            {typeof(IInputSystem), _inputSystem},
            {typeof(IItemGenerator<IUnityItem>), _itemGenerator},
            {typeof(IJobsExecutor), new JobsExecutor()},
            {typeof(IItemSwapper<IUnityItem>), new AnimatedItemSwapper()},
            {typeof(IGameBoardSolver<IUnityItem>), new LinearGameBoardSolver()},
            {typeof(IBoardFillStrategy<IUnityItem>[]), GetBoardFillStrategies(_gameBoard, _itemGenerator)}
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

    private IBoardFillStrategy<IUnityItem>[] GetBoardFillStrategies(IGameBoard<IUnityItem> gameBoard, IItemGenerator<IUnityItem> itemGenerator)
    {
        return new IBoardFillStrategy<IUnityItem>[]
        {
            new ItemsScaleFillStrategy(gameBoard, itemGenerator),
            new ItemsDropFillStrategy(gameBoard, itemGenerator),
            new ItemsRollDownFillStrategy(gameBoard, itemGenerator)
        };
    }
}