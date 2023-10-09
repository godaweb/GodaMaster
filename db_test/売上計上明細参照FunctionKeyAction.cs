using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 売上計上明細参照FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 売上計上明細参照FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "売上計上明細参照FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            売上計上明細参照Form form = (売上計上明細参照Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
