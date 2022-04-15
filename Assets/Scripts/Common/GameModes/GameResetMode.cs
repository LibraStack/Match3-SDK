using System;
using Common.Extensions;
using Common.Interfaces;
using Match3.Infrastructure.Interfaces;

namespace Common.GameModes
{
    public class GameResetMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly Interfaces.IItemsPool<IUnityItem> _itemsPool;

        public GameResetMode(IAppContext appContext)
        {
            _unityGame = appContext.Resolve<UnityGame>();
            _itemsPool = appContext.Resolve<Interfaces.IItemsPool<IUnityItem>>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            _itemsPool.ReturnAllItems(_unityGame.GetGridSlots());
            _unityGame.ResetGameBoard();

            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}