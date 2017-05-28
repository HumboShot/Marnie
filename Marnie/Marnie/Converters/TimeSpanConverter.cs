using System;
using System.Globalization;
using Xamarin.Forms;

namespace Marnie
{
    public class TimeSpanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
                {
                    TimeSpan timeSpan = (TimeSpan) value;
                    return timeSpan.ToString();
                }
                catch (Exception)
                {
                    return null;
                }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
