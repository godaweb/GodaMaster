using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class EOSドラスタ受注取込FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public EOSドラスタ受注取込FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "EOSドラスタ受注取込FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            EOSドラスタ受注取込Form form = (EOSドラスタ受注取込Form)target.Parent;
            //form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
