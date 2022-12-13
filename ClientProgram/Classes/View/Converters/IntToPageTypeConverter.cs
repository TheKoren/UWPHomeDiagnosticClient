using System;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    /// <summary>
    /// Converter which helps to bind WindowsFrameStatus to the Frame in Mainpage.
    /// </summary>
    public class IntToPageTypeConverter : IValueConverter
    {
        /// <summary>
        /// Converting in to Page type.
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>Page type</returns>
        public object Convert(object intValue, Type targetType, object parameter, string language)
        {
            switch (intValue)
            {
                case 0: { return typeof(RoomPage); }
                case 1: { return typeof(ControlPage); }
                case 2: { return typeof(GraphPage); }
                case 3: { return typeof(HistoryPage); }
                default: return null;
            }
        }

        /// <summary>
        /// Converting Page to int
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
