using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Item : IReadableObject, IWritableObject
    {
        private string itemName;
        private uint itemCountWarehouse;
        private string itemUnit;
        private float itemUnitPrice;
        private string itemDescription;

        public string Name
        { get { return this.itemName; } }

        public uint CountWarehouse
        { get { return this.itemCountWarehouse; } }

        public string Unit
        { get { return this.itemUnit; } }

        public float UnitPrice
        { get { return this.itemUnitPrice; } }

        public string Description
        { get { return this.itemDescription; } }

        public Item(string name, uint countWarehouse, string unit, float unitPrice, string description)
        {
            this.itemName = name;
            this.itemCountWarehouse = countWarehouse;
            this.itemUnit = unit;
            this.itemUnitPrice = unitPrice;
            this.itemDescription = description;
        }

        public Item() { }

        private Item(ILoadManager man)
        {
            this.itemName = man.ReadLine().Split(':')[1];
            uint.TryParse(man.ReadLine().Split(':')[1], out this.itemCountWarehouse);
            this.itemUnit = man.ReadLine().Split(':')[1];
            float.TryParse(man.ReadLine().Split(':')[1], out this.itemUnitPrice);
            this.itemDescription = man.ReadLine().Split(':')[1];
            
        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"itemName:{this.itemName}");
            man.WriteLine($"itemCountWarehouse:{this.itemCountWarehouse}");
            man.WriteLine($"itemUnit:{this.itemUnit}");
            man.WriteLine($"itemUnitPrice:{this.itemUnitPrice}");
            man.WriteLine($"itemDescription:{this.itemDescription}");
        }

        public class Loader : IReadableObjectLoader
        {
            public Loader() { }
            public IReadableObject Load(ILoadManager man)
            {
                return new Item(man);
            }
        }
    }
}
