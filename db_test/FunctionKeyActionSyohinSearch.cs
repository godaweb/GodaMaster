using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class FunctionKeyActionSyohinSearch : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public FunctionKeyActionSyohinSearch(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "FunctionKeyActionSyohinSearch"; }
        }

        public void Execute(GcMultiRow target)
        {
            
            
            商品検索Form form = (商品検索Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
