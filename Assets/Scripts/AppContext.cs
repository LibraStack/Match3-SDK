using System;
using System.Collections.Generic;
using Implementation.Common;
using Implementation.Common.GameBoardSolvers;
using Implementation.Common.Interfaces;
using Implementation.ItemsDrop;
using Implementation.ItemsRollDown;
using Implementation.ItemsScale;
using Match3.Core.Interfaces;
using UnityEngine;

public class AppContext : MonoBehaviour, IAppContext, IDisposable
{
    [SerializeField] private GameUiCanvas _gameUiCanvas;
    [SerializeField] private ItemGenerator _itemGenerator;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private GameBoardRenderer _gameBoardRenderer;

    private Dictionary<Type, object> _registeredTypes;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>
        {
            { typeof(IGameUiCanvas), _gameUiCanvas },
            { typeof(IInputSystem), _inputSystem },
            { typeof(IGameBoardRenderer), _gameBoardRenderer },
            { typeof(IGameBoardDataProvider), _gameBoardRenderer },
            { typeof(IItemGenerator<IUnityItem>), _itemGenerator },
            { typeof(IItemSwapper<IUnityItem>), new AnimatedItemSwapper() },
            { typeof(IGameBoardSolver<IUnityItem>), new LinearGameBoardSolver() },
            { typeof(IBoardFillStrategy<IUnityItem>[]), GetBoardFillStrategies(_gameBoardRenderer, _itemGenerator) }
        };
    }

    public void Init()
    {
        _itemGenerator.InitPool(GetItemsCapacity());
    }

    public T Resolve<T>()
    {
        return (T)_registeredTypes[typeof(T)];
    }

    public void Dispose()
    {
        _itemGenerator.Dispose();
        _gameBoardRenderer.Dispose();
    }

    private int GetItemsCapacity()
    {
        return _gameBoardRenderer.RowCount * _gameBoardRenderer.ColumnCount +
               Mathf.Max(_gameBoardRenderer.RowCount, _gameBoardRenderer.ColumnCount) * 2;
    }

    private IBoardFillStrategy<IUnityItem>[] GetBoardFillStrategies(IGameBoardRenderer gameBoardRenderer,
        IItemGenerator<IUnityItem> itemGenerator)
    {
        return new IBoardFillStrategy<IUnityItem>[]
        {
            new ItemsScaleFillStrategy(gameBoardRenderer, itemGenerator),
            new ItemsDropFillStrategy(gameBoardRenderer, itemGenerator),
            new ItemsRollDownFillStrategy(gameBoardRenderer, itemGenerator)
        };
    }
}