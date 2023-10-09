﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 受注残集計_得意先FunctionKeyAction
    {
        private Keys m_KeyCode = Keys.None;

        public 受注残集計_得意先FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "受注残集計_得意先FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            受注残集計_得意先Form form = (受注残集計_得意先Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
