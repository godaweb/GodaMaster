using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class FunctionKeyActionSirsakiSearch : IAction
    {
        private Keys m_KeyCode = Keys.None;

        public FunctionKeyActionSirsakiSearch(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "FunctionKeyActionSirsakiSearch"; }
        }

        public void Execute(GcMultiRow target)
        {


            仕入先検索Form form = (仕入先検索Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
