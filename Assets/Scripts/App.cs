using AppStates;
using Interfaces;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;

    private IAppState _activeState;
    private PlayingState _playingState;
    private DrawGameBoardState _drawGameBoardState;

    private void Awake()
    {
        _playingState = new PlayingState(_appContext);
        _drawGameBoardState = new DrawGameBoardState();
    }

    private void Start()
    {
        ActivateState(_drawGameBoardState);
    }

    private void OnEnable()
    {
        _drawGameBoardState.Finished += DrawGameBoardStateOnFinished;
    }

    private void OnDisable()
    {
        _drawGameBoardState.Finished -= DrawGameBoardStateOnFinished;
    }

    private void ActivateState(IAppState state)
    {
        _activeState?.Deactivate();
        _activeState = state;
        _activeState.Activate();
    }

    private void DrawGameBoardStateOnFinished(object sender, int[,] gameBoardData)
    {
        _playingState.Configure(gameBoardData);
        ActivateState(_playingState);
    }
}