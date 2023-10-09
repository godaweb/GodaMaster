using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 商品問合せFunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 商品問合せFunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "FunctionKeyAction商品問合せ"; }
        }

        public void Execute(GcMultiRow target)
        {
            
            
            商品問合せForm form = (商品問合せForm)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
