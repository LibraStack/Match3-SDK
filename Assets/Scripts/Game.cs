using System;
using Implementation.Common.AppModes;
using Implementation.Common.Interfaces;

public class Game : IDisposable
{
    private readonly PlayingMode _playingMode;
    private readonly DrawGameBoardMode _drawGameBoardMode;

    private IAppMode _activeMode;

    public Game(IAppContext appContext)
    {
        _playingMode = new PlayingMode(appContext);
        _drawGameBoardMode = new DrawGameBoardMode(appContext);
    }

    public void Start()
    {
        ActivateMode(_drawGameBoardMode);
    }

    public void Enable()
    {
        _playingMode.Finished += OnPlayingModeFinished;
        _drawGameBoardMode.Finished += OnDrawGameBoardModeFinished;
    }

    public void Disable()
    {
        _playingMode.Finished -= OnPlayingModeFinished;
        _drawGameBoardMode.Finished -= OnDrawGameBoardModeFinished;
    }

    public void Dispose()
    {
        _playingMode.Dispose();
    }

    private void OnDrawGameBoardModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_playingMode);
    }

    private void OnPlayingModeFinished(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ActivateMode(IAppMode mode)
    {
        _activeMode?.Deactivate();
        _activeMode = mode;
        _activeMode.Activate();
    }
}