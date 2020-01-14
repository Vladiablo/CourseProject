using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Item
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
    }
}
