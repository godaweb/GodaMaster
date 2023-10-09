using System;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;

namespace db_test
{
    class CustomMoveToNextControl : IAction
    {
        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }
        public string DisplayName
        {
            get { return this.ToString(); }
        }
        public void Execute(GcMultiRow target)
        {
            Boolean isLastRow = (target.CurrentCellPosition.RowIndex == target.RowCount - 1);
            Boolean isLastCell = (target.CurrentCellPosition.CellIndex == target.Template.Row.Cells.Count - 2);
            if (!(isLastRow & isLastCell))
            {
                // 最後のセル以外のセルでは次のセルへ移動します。 
                SelectionActions.MoveToNextCell.Execute(target);
            }
            else
            {
                // 最後のセルでは次のコントロールへ移動します。 
                ComponentActions.SelectNextControl.Execute(target);
            }
        }
    }
}
