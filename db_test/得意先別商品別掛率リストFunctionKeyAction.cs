using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 得意先別商品別掛率リストFunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 得意先別商品別掛率リストFunctionKeyAction(Keys keyCode)
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
            得意先別商品別掛率リストForm form = (得意先別商品別掛率リストForm)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
