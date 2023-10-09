using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 出荷可能一覧表FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 出荷可能一覧表FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "出荷可能一覧表FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            出荷可能一覧表Form form = (出荷可能一覧表Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
