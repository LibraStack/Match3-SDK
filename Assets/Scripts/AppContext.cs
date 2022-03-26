using System;
using System.Collections.Generic;
using Implementation.Common;
using Implementation.Common.GameBoardSolvers;
using Implementation.Common.Interfaces;
using Implementation.Common.Models;
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

    [Space]
    [SerializeField] private IconsSetModel[] _iconSets;

    private Dictionary<Type, object> _registeredTypes;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>
        {
            { typeof(IInputSystem), _inputSystem },
            { typeof(IconsSetModel[]), _iconSets },
            { typeof(IGameUiCanvas), _gameUiCanvas },
            { typeof(IGameBoardRenderer), _gameBoardRenderer },
            { typeof(IGameBoardDataProvider), _gameBoardRenderer },
            { typeof(IItemGenerator), _itemGenerator },
            { typeof(IItemsPool<IUnityItem>), _itemGenerator },
            { typeof(IGameScoreBoard), new GameScoreBoard() },
            { typeof(ILevelGoalsProvider), new LevelGoalsProvider() },
            { typeof(IItemSwapper<IUnityItem>), new AnimatedItemSwapper() },
            { typeof(IGameBoardSolver<IUnityItem>), new LinearGameBoardSolver() },
            { typeof(IBoardFillStrategy<IUnityItem>[]), GetBoardFillStrategies(_gameBoardRenderer, _itemGenerator) }
        };
    }

    public T Resolve<T>()
    {
        return (T)_registeredTypes[typeof(T)];
    }

    private IBoardFillStrategy<IUnityItem>[] GetBoardFillStrategies(IGameBoardRenderer gameBoardRenderer,
        IItemsPool<IUnityItem> itemsPool)
    {
        return new IBoardFillStrategy<IUnityItem>[]
        {
            new ItemsScaleFillStrategy(gameBoardRenderer, itemsPool),
            new ItemsDropFillStrategy(gameBoardRenderer, itemsPool),
            new ItemsRollDownFillStrategy(gameBoardRenderer, itemsPool)
        };
    }
}