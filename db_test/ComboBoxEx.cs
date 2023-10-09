using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.ComponentModel;

namespace db_test
{
    /// <summary>
    /// ComboBoxEx
    /// </summary>
    public class ComboBoxEx : ComboBox
    {
        #region ComboBoxのBorderColor/BorderStyle実装

        private Color _borderColor = Color.Silver;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.None;
        private static int WM_PAINT = 0x000F;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, bounds, _borderColor, _borderStyle);
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        #endregion
    }
}
