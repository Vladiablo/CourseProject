using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public class Provider
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
    }
}
