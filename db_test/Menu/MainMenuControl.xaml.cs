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

namespace db_test.Menu
{
    /// <summary>
    /// MainMenuControl.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
        public event EventHandler<EventArgs> MenuItemClicked;


        public MainMenuControl()
        {
            InitializeComponent();

            DataContextChanged += (s, e) =>
            {
                if (DataContext != null)
                {
                    var items = FindName("items") as ItemsControl;
                    Keyboard.Focus(items);
                }
            };
        }

        protected virtual void OnMenuItemClicked(object sender, RoutedEventArgs e)
        {
            if (MenuItemClicked != null)
            {
                MenuItemClicked.Invoke(sender, e);
            }
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {

            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9)
            {
                var d = ((int)e.Key) - (int)Key.NumPad0 - 1;
                var data = DataContext as KeyValuePair<string, List<IMainMenuItem>>?;
                if (data.HasValue && data.Value.Value.Count > d)
                {
                    data.Value.Value[d].ShowForm();
                }
            }
        }
    }
}
