using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace db_test.Print
{
    public class PrintPreview : PrintPreviewControl
    {
        private bool isControlKeyDown = false;


        public PrintPreview()
        {
        }


        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            isControlKeyDown = e.Control;

            base.OnPreviewKeyDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (isControlKeyDown)
            {
                try
                {
                    Zoom = Math.Max(0.0d, Zoom + e.Delta * 0.001);
                }
                catch
                {
                }
            }

            base.OnMouseWheel(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            isControlKeyDown = false;
        }
    }
}
