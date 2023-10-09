using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace db_test.Print
{
    public partial class PrintPreviewDialog : Form
    {
        public PrintPreviewDialog()
        {
            InitializeComponent();
        }

        public PrintDocument Document
        {
            get { return printPreview.Document; }
            set { printPreview.Document = value; }
        }

        public bool Printed
        {
            get;
            protected set;
        }

        public Printer Printer
        {
            get;
            protected set;
        }


        public void ShowDialog(Printer printer)
        {
            ShowDialog(null, printer);
        }

        public void ShowDialog(IWin32Window owner, Printer printer)
        {
            Printer = printer;
            ShowDialog(owner);
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnPrintButtonClicked(object sender, EventArgs e)
        {
            Printer.Print(this);
            Printed = true;
        }

        private void OnZoomOriginalButtonClicked(object sender, EventArgs e)
        {
            printPreview.Zoom = 1.0d;
        }

        private void OnZoomSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                printPreview.Zoom = double.Parse(toolStripComboBoxZoom.Text.TrimEnd('%')) / 100;
            }
            catch
            {
            }
        }
    }
}
