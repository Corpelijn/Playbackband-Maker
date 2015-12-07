using PBB_Web.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using DocumentFormat.OpenXml;

namespace PBB_Web.Classes.Exporters.Files
{
    public static class ExcelSheetExporter
    {
        private static string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64)
                ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64)
                ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }

        private static Cell CreateTextCell(string header, UInt32 index, string text)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = header + index
            };
            

            var istring = new InlineString();
            var t = new Text { Text = text };
            istring.AppendChild(t);
            cell.AppendChild(istring);
            return cell;
        }

        public static byte[] GenerateExcel()
        {
            // Create a new spreadsheet in the RAM memory
            MemoryStream stream = new MemoryStream();
            SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            // Add a workbook part
            WorkbookPart workbookpart = document.AddWorkbookPart();
            // Add a workbook to the part
            workbookpart.Workbook = new Workbook();
            // Add a sheetpart to the workbookpart
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            // Add sheetdata to the sheet
            SheetData sheetData = new SheetData();
            // Add a worksheet to the worksheet part
            worksheetPart.Worksheet = new Worksheet(sheetData);


            Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Setup the information from the 
            Sheet sheet = new Sheet();
            sheet.Id = document.WorkbookPart.GetIdOfPart(worksheetPart);
            sheet.SheetId = 1;
            sheet.Name = "PBB";
            sheets.AppendChild(sheet);


            // Add header
            UInt32 rowIdex = 0;
            var row = new Row { RowIndex = ++rowIdex };
            sheetData.AppendChild(row);
            var cellIdex = 0;
            row.AppendChild(CreateTextCell(ColumnLetter(cellIdex++), rowIdex, "=NU()"));

            workbookpart.Workbook.Save();
            document.Close();

            return stream.ToArray();
        }

        private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column;
            column = new Column();
            column.Min = StartColumnIndex;
            column.Max = EndColumnIndex;
            column.Width = ColumnWidth;
            column.CustomWidth = true;
            return column;
        }
    }
}