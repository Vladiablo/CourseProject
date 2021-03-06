﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public static class Items
    {
        private static List<Item> items = new List<Item>();

        public static List<Item> GetItems
        { get { return items; } }

        public static uint Count
        { get { return (uint)items.Count; } }

        public static void AddItem(Item item)
        {
            items.Add(item);
        }

        public static bool FindItemByName(string itemName, out Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == itemName)
                {
                    item = items[i];
                    return true;
                }
            }
            item = new Item();
            return false;
        }

        public static void Clear()
        {
            items.Clear();
        }
    }
}
