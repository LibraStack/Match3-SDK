using System;
using Implementation.Common.Interfaces;
using Implementation.Common.Models;
using UnityEngine;

namespace Implementation.Common.AppModes
{
    public class GameInitMode : IAppMode, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IItemGenerator _itemGenerator;

        public event EventHandler Finished;

        public GameInitMode(IAppContext appContext)
        {
            _appContext = appContext;
            _itemGenerator = appContext.Resolve<IItemGenerator>();
        }

        public void Activate()
        {
            var gameData = _appContext.Resolve<IGameBoardDataProvider>().GetGameBoardData();
            var rowCount = gameData.GetLength(0);
            var columnCount = gameData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            var iconsSetIndex = _appContext.Resolve<IGameUiCanvas>().SelectedIconsSetIndex;
            var iconsSet = _appContext.Resolve<IconsSetModel[]>()[iconsSetIndex];

            _itemGenerator.CreateItems(iconsSet.Sprites, itemsPoolCapacity);

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _itemGenerator.Dispose();
        }
    }
}