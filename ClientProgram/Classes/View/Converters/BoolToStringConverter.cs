using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Search;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    internal class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return "Connected";
            return "Disconnected";
        }

        /// <summary>
        /// Converting opacity to boolean. Not used in current implementation.
        /// </summary>
        /// <param name="value">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>NotImplementedException</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
