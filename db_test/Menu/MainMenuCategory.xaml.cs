using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace db_test.Menu
{
    /// <summary>
    /// MainMenuCategory.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMenuCategory : UserControl
    {
        public event EventHandler<EventArgs> SelectedCategoryChanged;


        public MainMenuCategory()
        {
            InitializeComponent();
        }


        public KeyValuePair<string, List<IMainMenuItem>> SelectedCategory
        {
            get;
            protected set;
        }

        public string SelectedCategoryKey
        {
            get;
            protected set;
        }


        private void OnCategoryButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null)
            {
                var data = button.DataContext as KeyValuePair<string, List<IMainMenuItem>>?;
                if (data.HasValue)
                {
                    SelectedCategory = data.Value;
                    SelectedCategoryKey = data.Value.Key;
                    OnSelectedCategoryChanged();
                }
            }
        }

        protected virtual void OnSelectedCategoryChanged()
        {
            if (SelectedCategoryChanged != null)
            {
                SelectedCategoryChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnCategoryButtonPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Right)
            {
                var button = sender as ToggleButton;
                if (button != null)
                {
                    button.IsChecked = true;
                    OnCategoryButtonClicked(sender, e);

                    e.Handled = true;
                }
            }
        }
    }
}
