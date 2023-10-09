using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace db_test.Print
{
    public class Printer
    {
        public Printer()
        {
            Document = new PrintDocument();
            //Document.DefaultPageSettings.Margins = new Margins(30, 30, 30, 50);
        }


        public PrintDocument Document
        {
            get;
            set;
        }


        public bool Print()
        {
            return Print(null);
        }

        public bool Print(IWin32Window owner)
        {
            using (var printer = new PrintDialog())
            {
                if (printer.ShowDialog(owner) == DialogResult.OK)
                {
                    Document.PrinterSettings = printer.PrinterSettings;
                    Document.Print();
                    return true;
                }
            }
            return false;
        }

        public bool PrintPreview()
        {
            return PrintPreview(null);
        }

        public bool PrintPreview(IWin32Window owner)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = Document;
                preview.ShowDialog(owner, this);
                return preview.Printed;
            }
        }
    }
}
