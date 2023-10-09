using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 発注入力FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 発注入力FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "発注入力FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            発注入力Form form = (発注入力Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }


    }

}
