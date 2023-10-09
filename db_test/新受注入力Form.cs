using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace db_test
{
    public partial class 新受注入力Form : Form
    {
        public 新受注入力Form()
        {
            InitializeComponent();
        }

        private void 新受注入力Form_Load(object sender, EventArgs e)
        {
            gcMultiRow1.MultiSelect = false;
            gcMultiRow1.AllowUserToAddRows = false;
            gcMultiRow1.ScrollBars = ScrollBars.None;
            gcMultiRow2.MultiSelect = false;
            gcMultiRow2.AllowUserToAddRows = false;
            gcMultiRow2.ScrollBars = ScrollBars.None;
            gcMultiRow3.MultiSelect = false;
            gcMultiRow3.AllowUserToAddRows = true;
            gcMultiRow3.ScrollBars = ScrollBars.Vertical;
        }
    }
}
