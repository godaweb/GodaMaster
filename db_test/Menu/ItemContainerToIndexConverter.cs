using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace db_test.Menu
{
    public class ItemContainerToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var index = (int)value;
            var number = index + 1;
            if (number >= 10)
            {
                return string.Empty;
            }
            return string.Format("{0}.", number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
