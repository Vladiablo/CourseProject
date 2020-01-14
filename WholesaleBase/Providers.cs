using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    public static class Providers
    {
        private static List<Provider> providers = new List<Provider>();

        public static List<Provider> GetProviders
        { get { return providers; } }

        public static uint Count
        { get { return (uint)providers.Count; } }

        public static void AddProvider(Provider provider)
        {
            providers.Add(provider);
        }

        public static bool FindProviderByName(string providerName, out Provider provider)
        {
            for (int i = 0; i < providers.Count; i++)
            {
                if (providers[i].Name == providerName)
                {
                    provider = providers[i];
                    return true;
                }
            }
            provider = new Provider();
            return false;
        }

        public static void Clear()
        {
            providers.Clear();
        }
    }
}
