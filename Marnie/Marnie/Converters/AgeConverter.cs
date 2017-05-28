using System;
using System.Globalization;
using Xamarin.Forms;

namespace Marnie.Converters
{
    public class AgeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime birthDay = (DateTime)value;
                return DateTime.Now.Year - birthDay.Year;
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
