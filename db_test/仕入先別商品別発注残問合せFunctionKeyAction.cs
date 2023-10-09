using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 仕入先別商品別発注残問合せFunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 仕入先別商品別発注残問合せFunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "仕入先別商品別発注残問合せFunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            仕入先別商品別発注残問合せForm form = (仕入先別商品別発注残問合せForm)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
