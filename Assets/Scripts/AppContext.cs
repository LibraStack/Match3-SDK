using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.Models;
using Common.TileGroupDetectors;
using FillStrategies;
using Match3.App;
using Match3.App.Interfaces;
using Match3.Infrastructure;
using Match3.Infrastructure.Interfaces;
using Match3.Infrastructure.SequenceDetectors;
using Match3.UniTaskApp.Interfaces;
using UnityEngine;

public class AppContext : MonoBehaviour, IAppContext
{
    [SerializeField] private GameUiCanvas _gameUiCanvas;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private UnityGameBoardRenderer _gameBoardRenderer;

    [Space]
    [SerializeField] private GameObject _itemPrefab;

    [Space]
    [SerializeField] private IconsSetModel[] _iconSets;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>();

        RegisterInstance<IInputSystem>(_inputSystem);
        RegisterInstance<IconsSetModel[]>(_iconSets);
        RegisterInstance<IGameUiCanvas>(_gameUiCanvas);
        RegisterInstance<IUnityGameBoardRenderer, IGameBoardDataProvider<IUnityGridSlot>>(_gameBoardRenderer);

        RegisterInstance<UnityGame>(GetUnityGame());
        RegisterInstance<IUnityItemGenerator, IItemsPool<IUnityItem>>(GetItemGenerator());
        RegisterInstance<IBoardFillStrategy<IUnityGridSlot>[]>(GetBoardFillStrategies());
    }

    private Dictionary<Type, object> _registeredTypes;

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private void RegisterInstance<T>(T instance)
    {
        _registeredTypes.Add(typeof(T), instance);
    }

    private void RegisterInstance<T1, T2>(object instance)
    {
        _registeredTypes.Add(typeof(T1), instance);
        _registeredTypes.Add(typeof(T2), instance);
    }

    private UnityGame GetUnityGame()
    {
        var gameConfig = new GameConfig<IUnityGridSlot>
        {
            GameBoardDataProvider = _gameBoardRenderer,
            LevelGoalsProvider = new LevelGoalsProvider(),
            GameBoardSolver = GetGameBoardSolver(),
            SolvedSequencesConsumers = GetSolvedSequencesConsumers()
        };

        return new UnityGame(_inputSystem, _gameBoardRenderer, new AnimatedItemSwapper(), gameConfig);
    }

    private UnityItemGenerator GetItemGenerator()
    {
        return new UnityItemGenerator(_itemPrefab, new GameObject("ItemsPool").transform);
    }

    private IGameBoardSolver<IUnityGridSlot> GetGameBoardSolver()
    {
        return new GameBoardSolver<IUnityGridSlot>(new ISequenceDetector<IUnityGridSlot>[]
        {
            new VerticalLineDetector<IUnityGridSlot>(),
            new HorizontalLineDetector<IUnityGridSlot>()
        });
    }

    private ISolvedSequencesConsumer<IUnityGridSlot>[] GetSolvedSequencesConsumers()
    {
        return new ISolvedSequencesConsumer<IUnityGridSlot>[]
        {
            new GameScoreBoard(),
            new TileGroupDetector(_gameBoardRenderer)
        };
    }

    private IBoardFillStrategy<IUnityGridSlot>[] GetBoardFillStrategies()
    {
        return new IBoardFillStrategy<IUnityGridSlot>[]
        {
            new SimpleFillStrategy(this),
            new FallDownFillStrategy(this),
            new SlideDownFillStrategy(this)
        };
    }
}