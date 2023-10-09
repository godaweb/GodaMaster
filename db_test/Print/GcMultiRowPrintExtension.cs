using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    public static class GcMultiRowPrintExtension
    {
        public static bool ShowPrintPreview(this GcMultiRow gcMultiRow)
        {
            var printer = new Print.Printer();
            gcMultiRow.Document = printer.Document;
            return printer.PrintPreview(gcMultiRow);
        }

        public static bool ShowPrint(this GcMultiRow gcMultiRow)
        {
            var printer = new Print.Printer();
            gcMultiRow.Document = printer.Document;
            return printer.Print(gcMultiRow);
        }
    }
}
