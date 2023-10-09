using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 受注残集計_担当者FunctionKeyAction
    {
        private Keys m_KeyCode = Keys.None;

        public 受注残集計_担当者FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "受注残集計_担当者FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            受注残集計_担当者Form form = (受注残集計_担当者Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
