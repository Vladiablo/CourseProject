using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WholesaleBase
{
    public static class DataReader
    {

        private static string[] valuesInput = { "itemName", "itemUnit", "itemUnitPrice", "itemCountWarehouse", "itemDescription",
            "providerName", "providerAddress", "providerPhone", "contactFaceName", "supplyItemCount", "supplyCost", "supplyTerm"};

        private static string[] valuesInputStrings = { "Название товара:", "Единица измерения товара:", "Стоимость единицы товара:",
            "Количество товара на складе:", "Описание товара (Может быть пустым):", "Название поставщика:", "Адрес поставщика:",
            "Телефон поставщика:", "ФИО контактного лица:", "Количество товаров в поставке:", "Стоимость поставки:", "Срок поставки"};

        private static Item CreateItem(string itemName, uint itemCountWarehouse, string itemUnit, float itemUnitPrice, string itemDescription)
        {
            return new Item(itemName, itemCountWarehouse, itemUnit, itemUnitPrice, itemDescription);
        }

        private static Provider CreateProvider(string providerName, string providerAddress, Phone providerPhone, string contactFaceName)
        {
            return new Provider(providerName, providerAddress, providerPhone, contactFaceName);
        }

        private static Supply CreateSupply(Item supplyItem, uint supplyItemCount, Provider supplyProvider, float supplyCost, string supplyTerm)
        {
            return new Supply(supplyTerm, supplyItemCount, supplyCost, supplyItem, supplyProvider);
        }

        public static void ReadDataFromFile(string path, out uint items, out uint providers, out uint supplies)
        {
            items = 0;
            providers = 0;
            supplies = 0;

            FileInfo file;
            StreamReader reader;

            file = new FileInfo(path);

            if (!file.Exists) throw new Exception("Невозможно открыть файл.");
            reader = new StreamReader(file.OpenRead(), Encoding.Default);

            bool itemExists = false, providerExists = false;

            while (!reader.EndOfStream)
            {
                switch(reader.ReadLine().ToUpper())
                {
                    case "ITEM":
                        {
                            string itemName = reader.ReadLine();
                            if (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано название товара.");
                            }

                            string itemUnit = reader.ReadLine();
                            if (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                            {
                                Console.WriteLine("Некорректно указана единица измерения товара {0}.", itemName);
                            }

                            float itemUnitPrice;
                            bool successUnitPrice = float.TryParse(reader.ReadLine(), out itemUnitPrice);
                            if (!successUnitPrice)
                            {
                                itemUnitPrice = 0;
                                Console.WriteLine("Некорректно указана стоимость единицы товара {0}.", itemName);
                            }

                            uint itemCountWarehouse;
                            bool successCountWarehouse = uint.TryParse(reader.ReadLine(), out itemCountWarehouse);
                            if (!successCountWarehouse)
                            {
                                itemCountWarehouse = 0;
                                Console.WriteLine("Некорректно указано количество товара {0} на складе.", itemName);
                            }

                            string itemDescription = reader.ReadLine();

                            Items.AddItem(CreateItem(itemName, itemCountWarehouse, itemUnit, itemUnitPrice, itemDescription));
                            items++;
                            break;
                        }
                    case "ПОСТАВЩИК":
                        {
                            string providerName = reader.ReadLine();
                            if (providerName.StartsWith("\n") || providerName.StartsWith(" ") || providerName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано название поставщика.");
                            }

                            string providerAddress = reader.ReadLine();
                            if (providerAddress.StartsWith("\n") || providerAddress.StartsWith(" ") || providerAddress.Length == 0)
                            {
                                Console.WriteLine("Некорректно указан адрес поставщика {0}.", providerName);
                            }

                            Phone providerPhone;
                            ulong number;
                            bool successNumber = ulong.TryParse(reader.ReadLine(), out number);
                            if (!successNumber)
                            {
                                number = 0;
                                Console.WriteLine("Некорректно указан номер телефона поставщика {0}", providerName);
                            }
                            try
                            {
                                providerPhone = new Phone(number);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Ошибка в поставщике {1}:\n{0}", e.Message, providerName);
                                providerPhone = new Phone(0);
                            }

                            string contactFaceName = reader.ReadLine();
                            if (contactFaceName.StartsWith("\n") || contactFaceName.StartsWith(" ") || contactFaceName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано имя контактного лица поставщика {0}.", providerName);
                            }

                            Providers.AddProvider(CreateProvider(providerName, providerAddress, providerPhone, contactFaceName));
                            providers++;
                            break;
                        }
                    case "ПОСТАВКА":
                        {
                            string itemName = reader.ReadLine();
                            if (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано название товара.");
                            }

                            Item item;
                            if (!Items.FindItemByName(itemName, out item) || !Char.IsDigit((char)reader.Peek()))
                            {
                                string itemUnit = reader.ReadLine();
                                if (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                                {
                                    Console.WriteLine("Некорректно указана единица измерения товара {0}.", itemName);
                                }

                                float itemUnitPrice;
                                bool successUnitPrice = float.TryParse(reader.ReadLine(), out itemUnitPrice);
                                if (!successUnitPrice)
                                {
                                    itemUnitPrice = 0;
                                    Console.WriteLine("Некорректно указана стоимость единицы товара {0}.", itemName);
                                }

                                uint itemCountWarehouse;
                                bool successCountWarehouse = uint.TryParse(reader.ReadLine(), out itemCountWarehouse);
                                if (!successCountWarehouse)
                                {
                                    itemCountWarehouse = 0;
                                    Console.WriteLine("Некорректно указано количество товара {0} на складе.", itemName);
                                }

                                string itemDescription = reader.ReadLine();
                                item = CreateItem(itemName, itemCountWarehouse, itemUnit, itemUnitPrice, itemDescription);
                                Items.AddItem(item);
                                items++;
                            }

                            uint supplyItemCount;
                            bool successSupplyItemCount = uint.TryParse(reader.ReadLine(), out supplyItemCount);
                            if (!successSupplyItemCount)
                            {
                                supplyItemCount = 0;
                                Console.WriteLine("Некорректно указано количество товара {0} в поставке.", itemName);
                            }

                            string providerName = reader.ReadLine();
                            if (providerName.StartsWith("\n") || providerName.StartsWith(" ") || providerName.Length == 0)
                            {
                                Console.WriteLine("Некорректно указано название поставщика.");
                            }

                            Provider provider;
                            if(!Providers.FindProviderByName(providerName, out provider) || !Char.IsDigit((char)reader.Peek()))
                            {
                                string providerAddress = reader.ReadLine();
                                if (providerAddress.StartsWith("\n") || providerAddress.StartsWith(" ") || providerAddress.Length == 0)
                                {
                                    Console.WriteLine("Некорректно указан адрес поставщика {0}.", providerName);
                                }

                                Phone providerPhone;
                                ulong number;
                                bool successNumber = ulong.TryParse(reader.ReadLine(), out number);
                                if (!successNumber)
                                {
                                    number = 0;
                                    Console.WriteLine("Некорректно указан номер телефона поставщика {0}", providerName);
                                }
                                try
                                {
                                    providerPhone = new Phone(number);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Ошибка в поставщике {1}:\n{0}", e.Message, providerName);
                                    providerPhone = new Phone(0);
                                }

                                string contactFaceName = reader.ReadLine();
                                if (contactFaceName.StartsWith("\n") || contactFaceName.StartsWith(" ") || contactFaceName.Length == 0)
                                {
                                    Console.WriteLine("Некорректно указано имя контактного лица поставщика {0}.", providerName);
                                }

                                provider = CreateProvider(providerName, providerAddress, providerPhone, contactFaceName);
                                Providers.AddProvider(provider);
                                providers++;
                            }

                            float supplyCost;
                            bool successSupplyCost = float.TryParse(reader.ReadLine(), out supplyCost);
                            if (!successSupplyCost)
                            {
                                supplyCost = 0;
                                Console.WriteLine("Некорректно указана стоимость поставки товара {0} от поставщика {1}.", itemName, providerName);
                            }

                            string supplyTerm = reader.ReadLine();
                            if (supplyTerm.StartsWith("\n") || supplyTerm.StartsWith(" ") || supplyTerm.Length == 0)
                            {
                                Console.WriteLine("Некорректно указан срок поставки товара {0} от поставщика {1}.", itemName, providerName);
                            }

                            Supply supply = CreateSupply(item, supplyItemCount, provider, supplyCost, supplyTerm);
                            Supplies.AddSupply(supply);
                            supplies++;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private static string Input
        {
            get
            {
                string input = Console.ReadLine();
                if (input.ToUpper() == "ОТМЕНА") throw new Exception("Отмена ввода.");
                else return input;
            }
        }

        public static Item ReadItemFromConsole()
        {
            string itemName = "";
            string itemUnit = "";
            float itemUnitPrice = 0;
            uint itemCountWarehouse = 0;
            string itemDescription = "";

            Console.WriteLine("\"отмена\" - отмена ввода.");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(valuesInputStrings[i]);

                switch (valuesInput[i])
                {
                    case "itemName":
                        {
                            itemName = Input;
                            while (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                            {
                                Console.WriteLine("Название товара не должно начинаться с пробела или быть пустым. Повторите ввод.");
                                itemName = Input;
                            }
                            break;
                        }
                    case "itemUnit":
                        {
                            itemUnit = Input;
                            while (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                            {
                                Console.WriteLine("Единица измерения товара не должна начинаться с пробела или быть пустой. Повторите ввод.");
                                itemUnit = Input;
                            }
                            break;
                        }
                    case "itemUnitPrice":
                        {
                            bool successUnitPrice = float.TryParse(Input, out itemUnitPrice);
                            while (!successUnitPrice)
                            {
                                Console.WriteLine("Некорректно указана цена единицы товара. Повторите ввод.");
                                successUnitPrice = float.TryParse(Input, out itemUnitPrice);
                            }
                            break;
                        }
                    case "itemCountWarehouse":
                        {
                            bool successCountWarehouse = uint.TryParse(Input, out itemCountWarehouse);
                            while (!successCountWarehouse)
                            {
                                Console.WriteLine("Некорректно указано количество товара на складе. Повторите ввод.");
                                successCountWarehouse = uint.TryParse(Input, out itemCountWarehouse);
                            }
                            break;
                        }
                    case "itemDescription":
                        {
                            itemDescription = Input;
                            break;
                        }
                    default: 
                        {
                            break;
                        }
                }
            }

            Item item = CreateItem(itemName, itemCountWarehouse, itemUnit, itemUnitPrice, itemDescription);
            return item;
        }

        public static Provider ReadProviderFromConsole()
        {
            string providerName = "";
            string providerAddress = "";
            Phone providerPhone = new Phone();
            string contactFaceName = "";

            Console.WriteLine("\"отмена\" - отмена ввода.");

            for (int i = 5; i < 9; i++)
            {
                Console.WriteLine(valuesInputStrings[i]);

                switch (valuesInput[i])
                {
                    case "providerName":
                        {
                            providerName = Input;
                            while (providerName.StartsWith("\n") || providerName.StartsWith(" ") || providerName.Length == 0)
                            {
                                Console.WriteLine("Название производителя не должно начинаться с пробела или быть пустым. Повторите ввод.");
                                providerName = Input;
                            }
                            break;
                        }
                    case "providerAddress":
                        {
                            providerAddress = Input;
                            while (providerAddress.StartsWith("\n") || providerAddress.StartsWith(" ") || providerAddress.Length == 0)
                            {
                                Console.WriteLine("Адрес помтавщика не может начинаться с пробела или быть пустым. Повторите ввод.");
                                providerAddress = Input;
                            }
                            break;
                        }
                    case "providerPhone":
                        {
                            ulong number;
                            bool successNumber = ulong.TryParse(Input, out number);
                            try
                            {
                                providerPhone = new Phone(number);
                                successNumber = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                successNumber = false;
                            }
                            while (!successNumber)
                            {
                                Console.WriteLine("Некорректно указан номер телефона поставщика. Повторите ввод.");
                                successNumber = ulong.TryParse(Input, out number);
                                try
                                {
                                    providerPhone = new Phone(number);
                                    successNumber = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.WriteLine("Повторите ввод.");
                                    successNumber = false;
                                }
                            }
                            break;
                        }
                    case "contactFaceName":
                        {
                            contactFaceName = Input;
                            while (contactFaceName.StartsWith("\n") || contactFaceName.StartsWith(" ") || contactFaceName.Length == 0)
                            {
                                Console.WriteLine("Имя контактного лица не должно быть пустым или начинаться с пробела. Повторите ввод.");
                                contactFaceName = Input;
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            Provider provider = CreateProvider(providerName, providerAddress, providerPhone, contactFaceName);
            return provider;
        }

        public static Supply ReadSupplyFromConsole()
        {
            string itemName = "";
            string itemUnit = "";
            float itemUnitPrice = 0;
            uint itemCountWarehouse = 0;
            string itemDescription = "";

            string providerName = "";
            string providerAddress = "";
            Phone providerPhone = new Phone();
            string contactFaceName = "";

            string supplyTerm = "";
            uint supplyItemCount = 0;
            float supplyCost = 0;

            bool itemExists = false, providerExists = false;

            Item item = new Item();
            Provider provider = new Provider();

            Console.WriteLine("\"отмена\" - отмена ввода.");

            for (int i = 0; i < valuesInput.Length; i++)
            {
                Console.WriteLine(valuesInputStrings[i]);

                switch (valuesInput[i])
                {
                    case "itemName":
                        {
                            itemName = Input;
                            while (itemName.StartsWith("\n") || itemName.StartsWith(" ") || itemName.Length == 0)
                            {
                                Console.WriteLine("Название товара не должно начинаться с пробела или быть пустым. Повторите ввод.");
                                itemName = Input;
                            }
                            if(Items.FindItemByName(itemName, out item))
                            {
                                Console.WriteLine("Товар {0} уже существует. Продолжить ввод? \"да\" - продолжить, остальное - подстановка значения из базы.", itemName);
                                if (!(Console.ReadLine().ToUpper() == "ДА"))
                                {
                                    itemExists = true;
                                    i += 4;
                                }
                            }
                            break;
                        }
                    case "itemUnit":
                        {
                            itemUnit = Input;
                            while (itemUnit.StartsWith("\n") || itemUnit.StartsWith(" ") || itemUnit.Length == 0)
                            {
                                Console.WriteLine("Единица измерения товара не должна начинаться с пробела или быть пустой. Повторите ввод.");
                                itemUnit = Input;
                            }
                            break;
                        }
                    case "itemUnitPrice":
                        {
                            bool successUnitPrice = float.TryParse(Input, out itemUnitPrice);
                            while (!successUnitPrice)
                            {
                                Console.WriteLine("Некорректно указана цена единицы товара. Повторите ввод.");
                                successUnitPrice = float.TryParse(Input, out itemUnitPrice);
                            }
                            break;
                        }
                    case "itemCountWarehouse":
                        {
                            bool successCountWarehouse = uint.TryParse(Input, out itemCountWarehouse);
                            while (!successCountWarehouse)
                            {
                                Console.WriteLine("Некорректно указано количество товара на складе. Повторите ввод.");
                                successCountWarehouse = uint.TryParse(Input, out itemCountWarehouse);
                            }
                            break;
                        }
                    case "itemDescription":
                        {
                            itemDescription = Input;
                            break;
                        }


                    case "providerName":
                        {
                            providerName = Input;
                            while (providerName.StartsWith("\n") || providerName.StartsWith(" ") || providerName.Length == 0)
                            {
                                Console.WriteLine("Название производителя не должно начинаться с пробела или быть пустым. Повторите ввод.");
                                providerName = Input;
                            }
                            if (Providers.FindProviderByName(providerName, out provider))
                            {
                                Console.WriteLine("Поставщик {0} уже существует. Продолжить ввод? \"да\" - продолжить, остальное - подстановка значения из базы.", providerName);
                                if (!(Console.ReadLine().ToUpper() == "ДА"))
                                {
                                    providerExists = true;
                                    i += 3;
                                }
                            }
                            break;
                        }
                    case "providerAddress":
                        {
                            providerAddress = Input;
                            while (providerAddress.StartsWith("\n") || providerAddress.StartsWith(" ") || providerAddress.Length == 0)
                            {
                                Console.WriteLine("Адрес помтавщика не может начинаться с пробела или быть пустым. Повторите ввод.");
                                providerAddress = Input;
                            }
                            break;
                        }
                    case "providerPhone":
                        {
                            ulong number;
                            bool successNumber = ulong.TryParse(Input, out number);
                            try
                            {
                                providerPhone = new Phone(number);
                                successNumber = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                successNumber = false;
                            }
                            while (!successNumber)
                            {
                                Console.WriteLine("Некорректно указан номер телефона поставщика. Повторите ввод.");
                                successNumber = ulong.TryParse(Input, out number);
                                try
                                {
                                    providerPhone = new Phone(number);
                                    successNumber = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.WriteLine("Повторите ввод.");
                                    successNumber = false;
                                }
                            }
                            break;
                        }
                    case "contactFaceName":
                        {
                            contactFaceName = Input;
                            while (contactFaceName.StartsWith("\n") || contactFaceName.StartsWith(" ") || contactFaceName.Length == 0)
                            {
                                Console.WriteLine("Имя контактного лица не должно быть пустым или начинаться с пробела. Повторите ввод.");
                                contactFaceName = Input;
                            }
                            break;
                        }


                    case "supplyTerm":
                        {
                            supplyTerm = Input;
                            while (supplyTerm.StartsWith("\n") || supplyTerm.StartsWith(" ") || supplyTerm.Length == 0)
                            {
                                Console.WriteLine("Срок поставки не должен начинаться с пробела или быть пустым. Повторите ввод.");
                                supplyTerm = Input;
                            }
                            break;
                        }
                    case "supplyItemCount":
                        {
                            bool successSupplyItemCount = uint.TryParse(Input, out supplyItemCount);
                            while (!successSupplyItemCount)
                            {
                                Console.WriteLine("Некорректно указано количество товара в поставке. Повторите ввод.");
                                successSupplyItemCount = uint.TryParse(Input, out supplyItemCount);
                            }
                            break;
                        }
                    case "supplyCost":
                        {
                            bool successSupplyCost = float.TryParse(Input, out supplyCost);
                            while (!successSupplyCost)
                            {
                                Console.WriteLine("Некорректно указана стоимость поставки. Повторите ввод.");
                                successSupplyCost = float.TryParse(Input, out supplyCost);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            if (!itemExists)
            {
                item = CreateItem(itemName, itemCountWarehouse, itemUnit, itemUnitPrice, itemDescription);
                Items.AddItem(item);
            }
            if (!providerExists)
            {
                provider = CreateProvider(providerName, providerAddress, providerPhone, contactFaceName);
                Providers.AddProvider(provider);
            }
            Supply supply = CreateSupply(item, supplyItemCount, provider, supplyCost, supplyTerm);
            return supply;
        }

    }
}
