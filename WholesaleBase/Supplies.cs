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
        private static SaveManager supplySaver = new SaveManager("supplies.txt");

        public static List<Supply> GetSupplies
        { get { return supplies; } }

        public static uint Count
        { get { return (uint)supplies.Count; } }

        public static void AddSupply(Supply supply)
        {
            supplies.Add(supply);
        }

        public static void SaveSupplies()
        {
            supplySaver.CreateFile();
            foreach (Supply supply in supplies)
                supplySaver.WriteObject(supply);
        }

        public static void LoadSupplies()
        {
            Clear();
            LoadManager supplyLoader = new LoadManager("supplies.txt");
            supplyLoader.BeginRead();
            while (supplyLoader.IsLoading)
                supplies.Add(supplyLoader.Read(new Supply.Loader()));
            supplyLoader.EndRead();
        }

        public static void Clear()
        {
            supplies.Clear();
        }

    }
}
