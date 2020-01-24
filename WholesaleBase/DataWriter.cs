using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace WholesaleBase
{
    public static class DataWriter
    {
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private static void AddCell(string column, uint row, WorksheetPart worksheetPart, string value, CellValues dataType)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            Cell cell = InsertCellInWorksheet(column, row, worksheetPart);

            cell.CellValue = new CellValue(value);
            cell.DataType = dataType;
            worksheet.Save();
        }

        private static void MergeCells(WorksheetPart worksheetPart, string cell1Name, string cell2Name)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            MergeCells mergeCells;

            if (worksheet.Elements<MergeCells>().Count() > 0)
                mergeCells = worksheet.Elements<MergeCells>().First();
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.  
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                else
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
            }

            // Create the merged cell and append it to the MergeCells collection.  
            MergeCell mergeCell = new MergeCell()
            {
                Reference =
                new StringValue(cell1Name + ":" + cell2Name)
            };
            mergeCells.Append(mergeCell);
            worksheet.Save();
        }

        public static void ExportDataToOXML(string fileName)
        {
            SpreadsheetDocument document;
            try
            {
                document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Невозможно создать файл.");
            }
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart2.Worksheet = new Worksheet(new SheetData());

            WorksheetPart worksheetPart3 = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart3.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Товары" };
            sheets.Append(sheet);

            Sheet sheet2 = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart2), SheetId = 2, Name = "Поставщики" };
            sheets.Append(sheet2);

            Sheet sheet3 = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart3), SheetId = 3, Name = "Поставки" };
            sheets.Append(sheet3);

            AddCell("A", 1, worksheetPart, "Название", CellValues.String);
            AddCell("B", 1, worksheetPart, "Количество на складе", CellValues.String);
            AddCell("C", 1, worksheetPart, "Единица измерения", CellValues.String);
            AddCell("D", 1, worksheetPart, "Стоимость единицы", CellValues.String);
            AddCell("E", 1, worksheetPart, "Описание", CellValues.String);

            for (int i = 0; i < Items.Count; i++)
            {
                AddCell("A", (uint)i + 2, worksheetPart, Items.GetItems[i].Name, CellValues.String);
                AddCell("B", (uint)i + 2, worksheetPart, Items.GetItems[i].CountWarehouse.ToString(), CellValues.Number);
                AddCell("C", (uint)i + 2, worksheetPart, Items.GetItems[i].Unit, CellValues.String);
                AddCell("D", (uint)i + 2, worksheetPart, Items.GetItems[i].UnitPrice.ToString(), CellValues.Number);
                AddCell("E", (uint)i + 2, worksheetPart, Items.GetItems[i].Description, CellValues.String);
            }

            AddCell("A", 1, worksheetPart2, "Название", CellValues.String);
            AddCell("B", 1, worksheetPart2, "Адрес", CellValues.String);
            AddCell("C", 1, worksheetPart2, "Телефон", CellValues.String);
            AddCell("D", 1, worksheetPart2, "ФИО контактного лица", CellValues.String);

            for (int i = 0; i < Providers.Count; i++)
            {
                AddCell("A", (uint)i + 2, worksheetPart2, Providers.GetProviders[i].Name, CellValues.String);
                AddCell("B", (uint)i + 2, worksheetPart2, Providers.GetProviders[i].Address, CellValues.String);
                AddCell("C", (uint)i + 2, worksheetPart2, Providers.GetProviders[i].Phone.ToString(), CellValues.String);
                AddCell("D", (uint)i + 2, worksheetPart2, Providers.GetProviders[i].ContactFaceName, CellValues.String);
            }

            AddCell("A", 1, worksheetPart3, "Детали поставки", CellValues.String);
            MergeCells(worksheetPart3, "A1", "C1");

            AddCell("D", 1, worksheetPart3, "Поставщик", CellValues.String);
            MergeCells(worksheetPart3, "D1", "G1");

            AddCell("H", 1, worksheetPart3, "Товар", CellValues.String);
            MergeCells(worksheetPart3, "H1", "L1");

            AddCell("A", 2, worksheetPart3, "Срок поставки", CellValues.String);
            AddCell("B", 2, worksheetPart3, "Количество товаров", CellValues.String);
            AddCell("C", 2, worksheetPart3, "Стоимость поставки", CellValues.String);

            AddCell("D", 2, worksheetPart3, "Название поставщика", CellValues.String);
            AddCell("E", 2, worksheetPart3, "Адрес", CellValues.String);
            AddCell("F", 2, worksheetPart3, "Телефон", CellValues.String);
            AddCell("G", 2, worksheetPart3, "ФИО контактного лица", CellValues.String);

            AddCell("H", 2, worksheetPart3, "Название товара", CellValues.String);
            AddCell("I", 2, worksheetPart3, "Количество на складе", CellValues.String);
            AddCell("J", 2, worksheetPart3, "Единица измерения", CellValues.String);
            AddCell("K", 2, worksheetPart3, "Стоимость единицы", CellValues.String);
            AddCell("L", 2, worksheetPart3, "Описание", CellValues.String);

            for (int i = 0; i < Supplies.Count; i++)
            {
                AddCell("A", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Term, CellValues.String);
                AddCell("B", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].ItemCount.ToString(), CellValues.Number);
                AddCell("C", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Cost.ToString(), CellValues.Number);

                AddCell("D", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Provider.Name, CellValues.String);
                AddCell("E", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Provider.Address, CellValues.String);
                AddCell("F", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Provider.Phone.ToString(), CellValues.String);
                AddCell("G", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Provider.ContactFaceName, CellValues.String);

                AddCell("H", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Item.Name, CellValues.String);
                AddCell("I", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Item.CountWarehouse.ToString(), CellValues.Number);
                AddCell("J", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Item.Unit, CellValues.String);
                AddCell("K", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Item.UnitPrice.ToString(), CellValues.String);
                AddCell("L", (uint)i + 3, worksheetPart3, Supplies.GetSupplies[i].Item.Description, CellValues.String);
            }

            workbookPart.Workbook.Save();

            document.Close();
        }

        public static void SaveDataToTXT(string fileName)
        {
            StreamWriter sw;

            try
            {
                sw = new StreamWriter(File.Create(fileName), Encoding.Default);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Невозможно создать файл.");
            }

            for(int i = 0; i < Items.Count; i++)
            {
                sw.WriteLine("ТОВАР");
                sw.WriteLine(Items.GetItems[i].Name);
                sw.WriteLine(Items.GetItems[i].Unit);
                sw.WriteLine(Items.GetItems[i].UnitPrice.ToString());
                sw.WriteLine(Items.GetItems[i].CountWarehouse.ToString());
                sw.WriteLine(Items.GetItems[i].Description);
                sw.WriteLine();
            }

            for (int i = 0; i < Providers.Count; i++)
            {
                sw.WriteLine("ПОСТАВЩИК");
                sw.WriteLine(Providers.GetProviders[i].Name);
                sw.WriteLine(Providers.GetProviders[i].Address);
                sw.WriteLine(Providers.GetProviders[i].Phone.RawString);
                sw.WriteLine(Providers.GetProviders[i].ContactFaceName);
                sw.WriteLine();
            }

            for (int i = 0; i < Supplies.Count; i++)
            {
                sw.WriteLine("ПОСТАВКА");
                sw.WriteLine(Supplies.GetSupplies[i].Item.Name);
                sw.WriteLine(Supplies.GetSupplies[i].ItemCount.ToString());
                sw.WriteLine(Supplies.GetSupplies[i].Provider.Name);
                sw.WriteLine(Supplies.GetSupplies[i].Cost.ToString());
                sw.WriteLine(Supplies.GetSupplies[i].Term);
                sw.WriteLine();
            }

            sw.Close();
        }
    }
}
