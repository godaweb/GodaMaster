using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 伝票検索計上FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 伝票検索計上FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "伝票検索計上FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            伝票検索計上Form form = (伝票検索計上Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
