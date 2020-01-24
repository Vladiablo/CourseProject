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
        private static SaveManager providerSaver = new SaveManager("providers.txt");

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

        public static void SaveProviders()
        {
            providerSaver.CreateFile();
            foreach (Provider provider in providers)
                providerSaver.WriteObject(provider);
        }

        public static void LoadProviders()
        {
            Clear();
            LoadManager providerLoader = new LoadManager("providers.txt");
            providerLoader.BeginRead();
            while (providerLoader.IsLoading)
                providers.Add(providerLoader.Read(new Provider.Loader()) as Provider);
            providerLoader.EndRead();
        }

        public static void Clear()
        {
            providers.Clear();
        }
    }
}
