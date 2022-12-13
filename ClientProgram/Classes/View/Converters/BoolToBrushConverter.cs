using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ClientProgram
{
    /// <summary>
    /// Converter to color the connection status LED.
    /// </summary>
    internal class BoolToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converting boolean type to Brush.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>The converted Brush value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((bool)value)
            {
                return new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                return new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }

        /// <summary>
        /// Converting Brush to boolean. Not used in current implementation.
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
