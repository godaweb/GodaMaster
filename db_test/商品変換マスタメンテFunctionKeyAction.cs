using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class 商品変換マスタメンテFunctionKeyAction : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public 商品変換マスタメンテFunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "商品変換マスタメンテFunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {


            商品変換マスタメンテForm form = (商品変換マスタメンテForm)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
