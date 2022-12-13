using System;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    /// <summary>
    /// Converter to determine the opacity of the logbox.
    /// </summary>
    internal class BoolToOpacityConverter : IValueConverter
    {
        /// <summary>
        /// Converting boolean type to opacity value.
        /// </summary>
        /// <param name="boolValue">Boolean value</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>The converted opacity value</returns>
        public object Convert(object boolValue, Type targetType, object parameter, string language)
        {
            if ((bool)boolValue)
                return (double)1.0;
            else
                return (double)0.2;

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
