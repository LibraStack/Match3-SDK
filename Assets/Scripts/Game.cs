using System;
using Implementation.Common.AppModes;
using Implementation.Common.Extensions;
using Implementation.Common.Interfaces;

public class Game : IDisposable
{
    private readonly DrawGameBoardMode _drawGameBoardMode;
    private readonly GameInitMode _gameInitMode;
    private readonly GamePlayMode _gamePlayMode;

    private readonly IGameUiCanvas _gameUiCanvas;

    private IAppMode _activeMode;

    public Game(IAppContext appContext)
    {
        _drawGameBoardMode = new DrawGameBoardMode(appContext);
        _gameInitMode = new GameInitMode(appContext);
        _gamePlayMode = new GamePlayMode(appContext);

        _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
    }

    public void Start()
    {
        ActivateMode(_drawGameBoardMode);
    }

    public void Enable()
    {
        _drawGameBoardMode.Finished += OnDrawGameBoardModeFinished;
        _gameInitMode.Finished += OnGameInitModeFinished;
        _gamePlayMode.Finished += OnGamePlayModeFinished;
    }

    public void Disable()
    {
        _drawGameBoardMode.Finished -= OnDrawGameBoardModeFinished;
        _gameInitMode.Finished -= OnGameInitModeFinished;
        _gamePlayMode.Finished -= OnGamePlayModeFinished;
    }

    public void Dispose()
    {
        _gameInitMode.Dispose();
        _drawGameBoardMode.Dispose();
    }

    private void OnDrawGameBoardModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_gameInitMode);
    }

    private void OnGameInitModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_gamePlayMode);
    }

    private void OnGamePlayModeFinished(object sender, EventArgs e)
    {
        _gamePlayMode.Deactivate();
        _gameUiCanvas.ShowMessage("Game finished!");
    }

    private void ActivateMode(IAppMode mode)
    {
        _activeMode?.Deactivate();
        _activeMode = mode;
        _activeMode.Activate();
    }
}