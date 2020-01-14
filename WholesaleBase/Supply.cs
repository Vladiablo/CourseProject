using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Supply
    {
        private string supplyTerm;
        private uint supplyItemCount;
        private float supplyCost;
        private Item supplyItem;
        private Provider supplyProvider;

        public string Term
        { get { return this.supplyTerm; } }

        public uint ItemCount
        { get { return this.supplyItemCount; } }

        public float Cost
        { get { return this.supplyCost; } }

        public Item Item
        { get { return this.supplyItem; } }

        public Provider Provider
        { get { return this.supplyProvider; } }

        public Supply(string term, uint itemCount, float cost, Item item, Provider provider)
        {
            this.supplyTerm = term;
            this.supplyItemCount = itemCount;
            this.supplyCost = cost;
            this.supplyItem = item;
            this.supplyProvider = provider;
        }
    }
}
