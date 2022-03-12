using System;
using System.Collections.Generic;
using DG.Tweening;
using FillStrategies;
using Interfaces;
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
            {typeof(IBoardFillStrategy[]), GetBoardFillStrategies(_gameBoard, _itemGenerator)}
        };
    }

    private void Start()
    {
        InitItemsPool();
        DOTween.SetTweensCapacity(200, 100);
    }

    public T Resolve<T>()
    {
        return (T)_registeredTypes[typeof(T)];
    }
    
    private void InitItemsPool()
    {
        _itemGenerator.InitPool(9 * 9 + 25);
        // _itemGenerator.InitPool(_gameBoard.RowCount * _gameBoard.ColumnCount + 25); // TODO: Think about it.
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