using AppModes;
using Interfaces;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;

    private IAppMode _activeMode;
    private PlayingMode _playingMode;
    private DrawGameBoardMode _drawGameBoardMode;

    private void Awake()
    {
        _playingMode = new PlayingMode(_appContext);
        _drawGameBoardMode = new DrawGameBoardMode();
    }

    private void Start()
    {
        ActivateMode(_drawGameBoardMode);
    }

    private void OnEnable()
    {
        _drawGameBoardMode.Finished += DrawGameBoardModeOnFinished;
    }

    private void OnDisable()
    {
        _drawGameBoardMode.Finished -= DrawGameBoardModeOnFinished;
    }

    private void ActivateMode(IAppMode mode)
    {
        _activeMode?.Deactivate();
        _activeMode = mode;
        _activeMode.Activate();
    }

    private void DrawGameBoardModeOnFinished(object sender, int[,] gameBoardData)
    {
        _playingMode.Configure(gameBoardData);
        ActivateMode(_playingMode);
    }
}