using System;
using Common.Extensions;
using Common.Interfaces;
using Match3.App;

namespace Common.AppModes
{
    public class GameResetMode : IAppMode
    {
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly Match3Game<IUnityItem> _match3Game;

        public event EventHandler Finished;

        public GameResetMode(IAppContext appContext)
        {
            _itemsPool = appContext.Resolve<IItemsPool<IUnityItem>>();
            _match3Game = appContext.Resolve<Match3Game<IUnityItem>>();
        }

        public void Activate()
        {
            _itemsPool.ReturnAllItems(_match3Game.GetGridSlots());
            _match3Game.ResetGameBoard();

            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}