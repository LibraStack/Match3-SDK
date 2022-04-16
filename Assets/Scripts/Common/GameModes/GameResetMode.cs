using System;
using Common.Extensions;
using Common.Interfaces;
using Match3.Infrastructure.Interfaces;

namespace Common.GameModes
{
    public class GameResetMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public GameResetMode(IAppContext appContext)
        {
            _unityGame = appContext.Resolve<UnityGame>();
            _itemsPool = appContext.Resolve<IItemsPool<IUnityItem>>();
            _gameBoardRenderer = appContext.Resolve<IUnityGameBoardRenderer>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            _itemsPool.ReturnAllItems(_unityGame.GetGridSlots());
            _gameBoardRenderer.ResetGridTiles();
            _unityGame.ResetGameBoard();

            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}