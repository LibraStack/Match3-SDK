using System.Collections.Generic;
using Common.Interfaces;
using Match3.Template.Interfaces;

namespace Common.Extensions
{
    public static class ItemsPoolExtensions
    {
        public static void ReturnAllItems(this IItemsPool<IUnityItem> itemsPool, IEnumerable<IUnityGridSlot> gridSlots)
        {
            foreach (var gridSlot in gridSlots)
            {
                if (gridSlot.Item == null)
                {
                    continue;
                }

                itemsPool.ReturnItem(gridSlot.Item);
                gridSlot.Item.Hide();
                gridSlot.Clear();
            }
        }
    }
}