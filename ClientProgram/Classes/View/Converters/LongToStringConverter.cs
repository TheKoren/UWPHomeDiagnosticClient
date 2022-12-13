using System;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    /// <summary>
    /// Converter to make unixTimeSeconds readable on the UI.
    /// </summary>
    public class LongToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converting UnixTimeStamp to string
        /// </summary>
        /// <param name="value">UnixTimeStamp</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>Timestamp in string format.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return DateTimeOffset.FromUnixTimeSeconds((long)value).ToString("yyyy. MM. dd.  HH:mm:ss");
        }
        /// <summary>
        /// Converting string to UnixTimeStamp
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
