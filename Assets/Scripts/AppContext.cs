using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.Solvers;
using DG.Tweening;
using ItemsDropImplementation;
using ItemsScaleImplementation;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AppContext : MonoBehaviour, IAppContext
{
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private GameCanvas _gameCanvas;
    [SerializeField] private ItemGenerator _itemGenerator;
    [SerializeField] private CanvasInputSystem _inputSystem;

    private Dictionary<Type, object> _registeredTypes;

    private void Awake()
    {
        _registeredTypes = new Dictionary<Type, object>
        {
            {typeof(IGameBoard), _gameBoard},
            {typeof(IGameCanvas), _gameCanvas},
            {typeof(IInputSystem), _inputSystem},
            {typeof(IItemGenerator), _itemGenerator},
            {typeof(IJobsExecutor), new JobsExecutor()},
            {typeof(IItemSwapper), new AnimatedItemSwapper()},
            {typeof(IGameBoardSolver), new LinearGameBoardSolver(_gameBoard)},
            {typeof(IBoardFillStrategy[]), GetBoardFillStrategies(_gameBoard, _itemGenerator)}
        };
    }

    private void Start()
    {
        Init();
        DOTween.SetTweensCapacity(200, 100);
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private void Init()
    {
        _gameBoard.Init(this);
        _itemGenerator.InitPool(_gameBoard.RowCount * _gameBoard.ColumnCount + 25);
    }

    private void Dispose()
    {
        _gameBoard.Dispose();
        _itemGenerator.Dispose();
    }

    private IBoardFillStrategy[] GetBoardFillStrategies(IGrid gameBoard, IItemGenerator itemGenerator)
    {
        return new IBoardFillStrategy[]
        {
            new ItemsScaleFillStrategy(gameBoard, itemGenerator),
            new ItemsDropFillStrategy(gameBoard, itemGenerator)
        };
    }
}