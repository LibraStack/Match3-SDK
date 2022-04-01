using System.Collections.Generic;
using Common.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Common.Extensions
{
    public static class ItemsPoolExtensions
    {
        public static void ReturnAllItems<TItem>(this IItemsPool<TItem> itemsPool,
            IEnumerable<GridSlot<TItem>> gridSlots) where TItem : IItem
        {
            foreach (var gridSlot in gridSlots)
            {
                if (gridSlot.Item == null)
                {
                    continue;
                }

                gridSlot.Item.Hide();
                itemsPool.ReturnItem(gridSlot.Item);
            }
        }
    }
}