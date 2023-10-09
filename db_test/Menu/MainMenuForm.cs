using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace db_test.Menu
{
    /// <summary>
    ///     メインメニューフォーム
    /// </summary>
    public partial class MainMenuForm : Form
    {
        private MainMenuViewModel model;


        /// <summary>
        ///     コンストラクタ。
        /// </summary>
        public MainMenuForm()
        {
            InitializeComponent();

            toolStripLabelOperatorName.Text = string.Format("({1}){0}", SelectOperatorDialog.Operator.Name, SelectOperatorDialog.Operator.Code);
            toolStripLabelCurrentTerm.Text = string.Format("{0}～{1}", string.Empty, string.Empty);

            model = new MainMenuViewModel();
            mainMenuCategory1.DataContext = model;
            mainMenuCategory1.SelectedCategoryChanged += OnSelectedCategoryChanged;
            mainMenuControl1.MenuItemClicked += OnMenuItemClicked;
            mainMenuControl1.PreviewKeyDown += OnMainMenuPreviewKeyDown;
        }



        /// <summary>
        ///     カテゴリメニューが選択されたとき。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedCategoryChanged(object sender, EventArgs e)
        {
            if (model.Categories.ContainsKey(mainMenuCategory1.SelectedCategoryKey))
            {
                mainMenuControl1.DataContext = mainMenuCategory1.SelectedCategory;
                mainMenuControl1.Focus();
            }
        }

        /// <summary>
        ///     メニューアイテムが選択されたとき。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                var item = button.DataContext as IMainMenuItem;
                if (item != null)
                {
                    item.ShowForm();
                }
            }
        }

        private void OnMainMenuPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                mainMenuCategory1.Focus();
            }
        }
    }
}
