using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleBase
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = new System.Text.UnicodeEncoding();
            Console.InputEncoding = new System.Text.UnicodeEncoding();

            //TODO: Доделать менеджеры загрузки и сохранения, добавить события для начала загрузки и сохранения и вывод в консоль

            while (true)
            {
                if (Items.Count > 0) Console.WriteLine("Товаров введено: {0}.", Items.Count);
                if (Providers.Count > 0) Console.WriteLine("Поставщиков введено: {0}.", Providers.Count);
                if (Supplies.Count > 0) Console.WriteLine("Поставок введено: {0}.", Supplies.Count);
                
                Console.WriteLine("Доступные команды: ");
                Console.WriteLine("\"ввод\" товар/поставщик/поставка - ввод указанных данных через консоль, \"импорт\" - импорт данных из файла;");
                Console.WriteLine("\"load\" - load data");
                if (Supplies.Count > 0 || Providers.Count > 0 || Items.Count > 0)
                {
                    Console.WriteLine("\"сохранить\" - сохранение данных в тестовом формате (.txt);");
                    Console.WriteLine("\"экспорт\" - экспорт данных в таблицу Open XML (MS Excel .xlsx);");
                    Console.WriteLine("\"очистить товар/поставщик/поставки/все\" - очистить выбранный список;");
                    Console.WriteLine("\"save\" - save data"); 
                }
                Console.Write("\"выход\" - выход из программы.");
                Console.WriteLine();
                string[] command = Console.ReadLine().ToUpper().Split();
                switch (command[0])
                {
                    case "ВВОД":
                        {
                            try
                            {
                                switch (command[1])
                                {
                                    case "ТОВАР":
                                        {
                                            try
                                            {
                                                Item item = DataReader.ReadItemFromConsole();
                                                Items.AddItem(item);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                break;
                                            }
                                        }
                                    case "ПОСТАВЩИК":
                                        {
                                            try
                                            {
                                                Provider provider = DataReader.ReadProviderFromConsole();
                                                Providers.AddProvider(provider);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                break;
                                            }
                                        }
                                    case "ПОСТАВКА":
                                        {
                                            try
                                            {
                                                Supply supply = DataReader.ReadSupplyFromConsole();
                                                Supplies.AddSupply(supply);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                break;
                                            }
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                break;
                            }
                            break;
                        }
                    case "ИМПОРТ":
                        {
                            while (true)
                            {
                                try
                                {
                                    string fileName;
                                    if (command.Length == 1)
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА" || fileName.ToUpper() == "CANCEL") break;
                                    }
                                    else if (command[1].Length == 0 || command[1].StartsWith(" "))
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА" || fileName.ToUpper() == "CANCEL") break;
                                    }
                                    else fileName = command[1];
                                    uint items, providers, supplies;
                                    DataReader.ReadDataFromFile(fileName, out items, out providers, out supplies);
                                    Console.WriteLine("Введено из файла:");
                                    Console.WriteLine("Товаров: {0};\nПоставщиков: {1};\nПоставок: {2}.\n", items, providers, supplies);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    if (command.Length >= 2)
                                    {
                                        command[1] = "";
                                    }
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        }

                    case "SAVE":
                        {
                            if (Items.Count == 0 && Providers.Count == 0 && Supplies.Count == 0) break;
                            Items.SaveItems();
                            Providers.SaveProviders();
                            Supplies.SaveSupplies();
                            break;
                        }

                    case "LOAD":
                        {
                            Items.LoadItems();
                            Providers.LoadProviders();
                            Supplies.LoadSupplies();
                            break;
                        }

                    case "СОХРАНИТЬ":
                        {
                            if (Items.Count == 0 || Providers.Count == 0 || Supplies.Count == 0) break;
                            while (true)
                            {
                                try
                                {
                                    string fileName;
                                    if (command.Length == 1)
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА") break;
                                    }
                                    else if (command[1].Length == 0 || command[1].StartsWith(" "))
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА") break;
                                    }
                                    else fileName = command[1];
                                    DataWriter.SaveDataToTXT(fileName);
                                    Console.WriteLine("Данные сохранены.");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    if (command.Length >= 2)
                                    {
                                        command[1] = "";
                                    }
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        }
                    case "ЭКСПОРТ":
                        {
                            if (Items.Count == 0 || Providers.Count == 0 || Supplies.Count == 0) break;
                            while (true)
                            {
                                try
                                {
                                    string fileName;
                                    if (command.Length == 1)
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА") break;
                                    }
                                    else if (command[1].Length == 0 || command[1].StartsWith(" "))
                                    {
                                        Console.WriteLine("Для возврата напишите \"отмена\".");
                                        Console.WriteLine("Имя файла: ");
                                        fileName = Console.ReadLine();
                                        if (fileName.ToUpper() == "ОТМЕНА") break;
                                    }
                                    else fileName = command[1];
                                    DataWriter.ExportDataToOXML(fileName);
                                    Console.WriteLine("Данные выведены.");
                                    break;
                                }
                                catch (Exception e)
                                {
                                    if (command.Length >= 2)
                                    {
                                        command[1] = "";
                                    }
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        }
                    case "ОЧИСТИТЬ":
                        {
                            try
                            {
                                switch (command[1])
                                {
                                    case "ТОВАР":
                                        {
                                            if (Items.Count == 0) break;
                                            Items.Clear();
                                            Console.WriteLine("Список товаров очищен.");
                                            break;
                                        }
                                    case "ПОСТАВЩИК":
                                        {
                                            if (Providers.Count == 0) break;
                                            Providers.Clear();
                                            Console.WriteLine("Список поставщиков очищен.");
                                            break;
                                        }
                                    case "ПОСТАВКИ":
                                        {
                                            if (Supplies.Count == 0) break;
                                            Supplies.Clear();
                                            Console.WriteLine("Список поставок очищен.");
                                            break;
                                        }
                                    case "ВСЕ":
                                        {
                                            if (Items.Count == 0 && Providers.Count == 0 && Supplies.Count == 0) break;
                                            Items.Clear();
                                            Providers.Clear();
                                            Supplies.Clear();
                                            Console.WriteLine("Все списки очищены.");
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                            catch(IndexOutOfRangeException e)
                            {
                                break;
                            }
                            break;
                        }
                    case "ВЫХОД":
                        {
                            System.Environment.Exit(1);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Неизвестная команда.");
                            break;
                        }
                }
            }
        }
    }
}
