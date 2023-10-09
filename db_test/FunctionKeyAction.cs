using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    public class FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            éÛíçì¸óÕForm form = (éÛíçì¸óÕForm)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
