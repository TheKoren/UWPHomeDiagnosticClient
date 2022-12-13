using System;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    /// <summary>
    /// Converter to any number type to string.
    /// </summary>
    internal class NumberToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converting number to string format.
        /// </summary>
        /// <param name="value">Number</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>Number in readable string format.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converting string to number.
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
