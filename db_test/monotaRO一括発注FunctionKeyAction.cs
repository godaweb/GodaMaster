using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class monotaRO一括発注FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public monotaRO一括発注FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "monotaRO一括発注FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            monotaRO一括発注Form form = (monotaRO一括発注Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
