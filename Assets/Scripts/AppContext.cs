using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.Models;
using Common.SequenceDetectors;
using Common.TileGroupDetectors;
using FillStrategies;
using Match3.App;
using Match3.App.Interfaces;
using UnityEngine;

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
        _registeredTypes = new Dictionary<Type, object>
        {
            { typeof(IInputSystem), _inputSystem },
            { typeof(IconsSetModel[]), _iconSets },
            { typeof(IGameUiCanvas), _gameUiCanvas },
            { typeof(IItemGenerator), _itemGenerator },
            { typeof(IItemsPool<IUnityItem>), _itemGenerator },
            { typeof(IGameBoardDataProvider), _gameBoardRenderer },
            { typeof(IUnityGameBoardRenderer), _gameBoardRenderer },
            { typeof(Match3Game<IUnityItem>), GetMatch3Game() },
            { typeof(IBoardFillStrategy<IUnityItem>[]), GetBoardFillStrategies(_gameBoardRenderer, _itemGenerator) }
        };
    }

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private Match3Game<IUnityItem> GetMatch3Game()
    {
        var gameConfig = new GameConfig<IUnityItem>
        {
            InputSystem = _inputSystem,
            GameBoardRenderer = _gameBoardRenderer,
            GameBoardDataProvider = _gameBoardRenderer,
            ItemSwapper = new AnimatedItemSwapper(),
            LevelGoalsProvider = new LevelGoalsProvider(),
            GameBoardSolver = GetGameBoardSolver(),
            SolvedSequencesConsumers = GetSolvedSequencesConsumers()
        };

        return new Match3Game<IUnityItem>(gameConfig);
    }

    private IGameBoardSolver<IUnityItem> GetGameBoardSolver()
    {
        return new GameBoardSolver(new ISequenceDetector<IUnityItem>[]
        {
            new VerticalLineDetector(),
            new HorizontalLineDetector()
        });
    }

    private ISolvedSequencesConsumer<IUnityItem>[] GetSolvedSequencesConsumers()
    {
        return new ISolvedSequencesConsumer<IUnityItem>[]
        {
            new GameScoreBoard(),
            new TileGroupDetector(_gameBoardRenderer)
        };
    }

    private IBoardFillStrategy<IUnityItem>[] GetBoardFillStrategies(IUnityGameBoardRenderer gameBoardRenderer,
        IItemsPool<IUnityItem> itemsPool)
    {
        return new IBoardFillStrategy<IUnityItem>[]
        {
            new SimpleFillStrategy(gameBoardRenderer, itemsPool),
            new FallDownFillStrategy(gameBoardRenderer, itemsPool),
            new SlideDownFillStrategy(gameBoardRenderer, itemsPool)
        };
    }
}