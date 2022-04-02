using System;
using Common.Interfaces;
using Common.Models;
using Match3.App;
using Match3.App.Interfaces;
using UnityEngine;

namespace Common.AppModes
{
    public class GameInitMode : IAppMode, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IconsSetModel[] _iconSets;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IItemGenerator _itemGenerator;
        private readonly Match3Game<IUnityItem> _match3Game;

        private bool _isInitialized;

        public event EventHandler Finished;

        public GameInitMode(IAppContext appContext)
        {
            _appContext = appContext;
            _iconSets = _appContext.Resolve<IconsSetModel[]>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _itemGenerator = appContext.Resolve<IItemGenerator>();
            _match3Game = appContext.Resolve<Match3Game<IUnityItem>>();
        }

        public void Activate()
        {
            const int level = 0;

            if (_isInitialized)
            {
                SetLevel(level);
            }
            else
            {
                Init(level);
                SetLevel(level);
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _match3Game.Dispose();
            _itemGenerator.Dispose();
        }

        private void Init(int level)
        {
            var gameBoardData = _appContext.Resolve<IGameBoardDataProvider>().GetGameBoardData(level);
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            _itemGenerator.CreateItems(itemsPoolCapacity);
            _isInitialized = true;
        }

        private void SetLevel(int level)
        {
            _match3Game.InitGameLevel(level);
            _itemGenerator.SetSprites(_iconSets[_gameUiCanvas.SelectedIconsSetIndex].Sprites);
        }
    }
}