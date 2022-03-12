using System;
using AppModes;
using Interfaces;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;

    private IAppMode _activeMode;
    private IAppMode _playingMode;
    private IAppMode _drawGameBoardMode;

    private void Awake()
    {
        _playingMode = new PlayingMode(_appContext);
        _drawGameBoardMode = new DrawGameBoardMode(_appContext);
    }

    private void Start()
    {
        ActivateMode(_drawGameBoardMode);
    }

    private void OnEnable()
    {
        _playingMode.Finished += OnPlayingModeFinished;
        _drawGameBoardMode.Finished += OnDrawGameBoardModeFinished;
    }

    private void OnDisable()
    {
        _playingMode.Finished -= OnPlayingModeFinished;
        _drawGameBoardMode.Finished -= OnDrawGameBoardModeFinished;
    }

    private void ActivateMode(IAppMode mode)
    {
        _activeMode?.Deactivate();
        _activeMode = mode;
        _activeMode.Activate();
    }

    private void OnDrawGameBoardModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_playingMode);
    }

    private void OnPlayingModeFinished(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}