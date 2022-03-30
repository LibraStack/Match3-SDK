using System;
using Common.Interfaces;
using Common.Models;
using Match3.App;
using UnityEngine;

namespace Common.AppModes
{
    public class GameInitMode : IAppMode, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IItemGenerator _itemGenerator;
        private readonly Match3Game<IUnityItem> _match3Game;

        public event EventHandler Finished;

        public GameInitMode(IAppContext appContext)
        {
            _appContext = appContext;
            _itemGenerator = appContext.Resolve<IItemGenerator>();
            _match3Game = appContext.Resolve<Match3Game<IUnityItem>>();
        }

        public void Activate()
        {
            var gameBoardData = _appContext.Resolve<IGameBoardDataProvider>().GetGameBoardData(0);
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            var iconsSetIndex = _appContext.Resolve<IGameUiCanvas>().SelectedIconsSetIndex;
            var iconsSet = _appContext.Resolve<IconsSetModel[]>()[iconsSetIndex];

            _match3Game.InitGameBoard(gameBoardData);
            _itemGenerator.CreateItems(iconsSet.Sprites, itemsPoolCapacity);

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _match3Game.Dispose();
            _itemGenerator.Dispose();
        }
    }
}