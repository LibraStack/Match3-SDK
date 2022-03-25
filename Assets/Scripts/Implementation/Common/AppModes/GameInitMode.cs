using System;
using Implementation.Common.Interfaces;
using Match3.Core.Interfaces;
using UnityEngine;

namespace Implementation.Common.AppModes
{
    public class GameInitMode : IAppMode, IDisposable
    {
        private readonly IGameBoardDataProvider _gameDataProvider;
        private readonly IItemGenerator<IUnityItem> _itemGenerator;

        public event EventHandler Finished;

        public GameInitMode(IAppContext appContext)
        {
            _itemGenerator = appContext.Resolve<IItemGenerator<IUnityItem>>();
            _gameDataProvider = appContext.Resolve<IGameBoardDataProvider>();
        }

        public void Activate()
        {
            var gameData = _gameDataProvider.GetGameBoardData();
            var rowCount = gameData.GetLength(0);
            var columnCount = gameData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            _itemGenerator.InitItemsPool(itemsPoolCapacity);
            
            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _itemGenerator.Dispose();
        }
    }
}