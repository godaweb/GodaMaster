using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class EOSドラスタ送信データ作成FunctionKeyAction
    {
        private Keys m_KeyCode = Keys.None;

        public EOSドラスタ送信データ作成FunctionKeyAction(Keys keyCode)
        {
            this.m_KeyCode = keyCode;
        }

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return "EOSドラスタ送信データ作成FunctionKeyAction"; }
        }

        public void Execute(GcMultiRow target)
        {
            EOSドラスタ送信データ作成Form form = (EOSドラスタ送信データ作成Form)target.Parent;
            form.FlushButton(this.m_KeyCode);
        }

        public Keys KeyCode
        {
            get { return m_KeyCode; }
            set { m_KeyCode = value; }
        }
    }
}
