using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Provider : IReadableObject, IWritableObject
    {
        private string providerName;
        private string providerAddress;
        private Phone providerPhone;
        private string contactFaceName;

        public string Name
        { get { return this.providerName; } }

        public string Address
        { get { return this.providerAddress; } }

        public Phone Phone
        { get { return this.providerPhone; } }

        public string ContactFaceName
        { get { return this.contactFaceName; } }

        public Provider(string name, string address, Phone phone, string contactFaceName)
        {
            this.providerName = name;
            this.providerAddress = address;
            this.providerPhone = phone;
            this.contactFaceName = contactFaceName;
        }

        public Provider() { }

        private Provider(ILoadManager man)
        {
            this.providerName = man.ReadLine().Split(':')[1];
            this.providerAddress = man.ReadLine().Split(':')[1];
            ulong phone;
            ulong.TryParse(man.ReadLine().Split(':')[1], out phone);
            try 
            {
                this.providerPhone = new Phone(phone);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
            this.contactFaceName = man.ReadLine().Split(':')[1];

        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"providerName:{this.providerName}");
            man.WriteLine($"providerAddress:{this.providerAddress}");
            man.WriteLine($"providerPhone:{this.providerPhone.RawString}");
            man.WriteLine($"contactFaceName:{this.contactFaceName}");
        }

        public class Loader : IReadableObjectLoader<Provider>
        {
            public Loader() { }
            public Provider Load(ILoadManager man)
            {
                return new Provider(man);
            }
        }
    }
}
