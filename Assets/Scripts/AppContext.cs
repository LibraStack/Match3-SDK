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

public class AppContext : MonoBehaviour, IAppContext
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
            { typeof(IInputSystem), _inputSystem },
            { typeof(IGameUiCanvas), _gameUiCanvas },
            { typeof(IGameBoardRenderer), _gameBoardRenderer },
            { typeof(IGameBoardDataProvider), _gameBoardRenderer },
            { typeof(IItemGenerator<IUnityItem>), _itemGenerator },
            { typeof(IItemSwapper<IUnityItem>), new AnimatedItemSwapper() },
            { typeof(IGameScoreBoard<IUnityItem>), new GameScoreBoard() },
            { typeof(IGameBoardSolver<IUnityItem>), new LinearGameBoardSolver() },
            { typeof(IBoardFillStrategy<IUnityItem>[]), GetBoardFillStrategies(_gameBoardRenderer, _itemGenerator) }
        };
    }

    public T Resolve<T>()
    {
        return (T)_registeredTypes[typeof(T)];
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