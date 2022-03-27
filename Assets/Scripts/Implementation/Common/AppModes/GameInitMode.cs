using System;
using Implementation.Common.Interfaces;
using Implementation.Common.Models;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common.AppModes
{
    public class GameInitMode : IAppMode, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IItemGenerator _itemGenerator;
        private readonly IGameBoard<IUnityItem> _gameBoard;

        public event EventHandler Finished;

        public GameInitMode(IAppContext appContext)
        {
            _appContext = appContext;
            _itemGenerator = appContext.Resolve<IItemGenerator>();
            _gameBoard = appContext.Resolve<IGameBoard<IUnityItem>>();
        }

        public void Activate()
        {
            var gameBoardData = _appContext.Resolve<IGameBoardDataProvider>().GetGameBoardData();
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            var iconsSetIndex = _appContext.Resolve<IGameUiCanvas>().SelectedIconsSetIndex;
            var iconsSet = _appContext.Resolve<IconsSetModel[]>()[iconsSetIndex];

            _gameBoard.Init(gameBoardData);
            _itemGenerator.CreateItems(iconsSet.Sprites, itemsPoolCapacity);

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _gameBoard.Dispose();
            _itemGenerator.Dispose();
        }
    }
}