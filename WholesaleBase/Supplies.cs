using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public static class Supplies
    {
        private static List<Supply> supplies = new List<Supply>();

        public static List<Supply> GetSupplies
        { get { return supplies; } }

        public static uint Count
        { get { return (uint)supplies.Count; } }

        public static void AddSupply(Supply supply)
        {
            supplies.Add(supply);
        }

        public static void Clear()
        {
            supplies.Clear();
        }

    }
}
