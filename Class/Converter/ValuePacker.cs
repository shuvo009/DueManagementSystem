using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;
namespace EasyShopManagement.Class.Converter
{
   public class ValuePacker :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ArrayList infromationCarrer = new ArrayList(values.Count());
            foreach (var value in values)
            {
                infromationCarrer.Add(value);
            }
            return infromationCarrer;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
