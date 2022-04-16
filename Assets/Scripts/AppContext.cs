using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.Models;
using Common.TileGroupDetectors;
using FillStrategies;
using Match3.Core.Interfaces;
using Match3.Infrastructure;
using Match3.Infrastructure.Interfaces;
using Match3.Infrastructure.SequenceDetectors;
using Match3.UnityApp;
using Match3.UnityApp.Interfaces;
using UnityEngine;
using IItemGenerator = Common.Interfaces.IItemGenerator;

public class AppContext : MonoBehaviour, IAppContext
{
    [SerializeField] private GameUiCanvas _gameUiCanvas;
    [SerializeField] private ItemGenerator _itemGenerator;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private UnityGameBoardRenderer _gameBoardRenderer;

    [Space]
    [SerializeField] private IconsSetModel[] _iconSets;

    private Dictionary<Type, object> _registeredTypes;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>();

        RegisterInstance<IInputSystem>(_inputSystem);
        RegisterInstance<IconsSetModel[]>(_iconSets);
        RegisterInstance<IGameUiCanvas>(_gameUiCanvas);
        RegisterInstance<IItemGenerator>(_itemGenerator);
        RegisterInstance<IItemsPool<IUnityItem>>(_itemGenerator);
        RegisterInstance<IUnityGameBoardRenderer>(_gameBoardRenderer);
        RegisterInstance<IGameBoardDataProvider<IUnityGridSlot>>(_gameBoardRenderer);
        RegisterInstance<UnityGame>(GetUnityGame());
        RegisterInstance<IBoardFillStrategy<IUnityGridSlot>[]>(GetBoardFillStrategies());
    }

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private void RegisterInstance<T>(T instance)
    {
        _registeredTypes.Add(typeof(T), instance);
    }

    private UnityGame GetUnityGame()
    {
        var gameConfig = new GameConfig<IUnityGridSlot>
        {
            GameBoardDataProvider = _gameBoardRenderer,
            ItemSwapper = new AnimatedItemSwapper(),
            LevelGoalsProvider = new LevelGoalsProvider(),
            GameBoardSolver = GetGameBoardSolver(),
            SolvedSequencesConsumers = GetSolvedSequencesConsumers()
        };

        return new UnityGame(_inputSystem, _gameBoardRenderer, gameConfig);
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