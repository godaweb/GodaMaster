using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 商品別受注出荷履歴照会FunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 商品別受注出荷履歴照会FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "商品別受注出荷履歴照会FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            商品別受注出荷履歴照会Form form = (商品別受注出荷履歴照会Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
