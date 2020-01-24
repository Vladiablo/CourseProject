using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Supply : IReadableObject, IWritableObject
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

        private Supply(ILoadManager man)
        {
            this.supplyTerm = man.ReadLine().Split(':')[1];
            uint.TryParse(man.ReadLine().Split(':')[1], out this.supplyItemCount);
            float.TryParse(man.ReadLine().Split(':')[1], out this.supplyCost);
            Items.FindItemByName(man.ReadLine().Split(':')[1], out this.supplyItem);
            Providers.FindProviderByName(man.ReadLine().Split(':')[1], out this.supplyProvider);
        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"supplyTerm:{this.supplyTerm}");
            man.WriteLine($"supplyItemCount:{this.supplyItemCount}");
            man.WriteLine($"supplyCost:{this.supplyCost}");
            man.WriteLine($"supplyItem:{this.supplyItem.Name}");
            man.WriteLine($"supplyProvider:{this.supplyProvider.Name}");
        }

        public class Loader : IReadableObjectLoader
        {
            public Loader() { }
            public IReadableObject Load(ILoadManager man)
            {
                return new Supply(man);
            }
        }
    }
}
