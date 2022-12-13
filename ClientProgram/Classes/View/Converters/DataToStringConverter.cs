using System;
using Windows.UI.Xaml.Data;

namespace ClientProgram
{
    /// <summary>
    /// Converter to display sensorvalues on the RoomPage.
    /// </summary>
    public class DataToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converting sensorvalue to string.
        /// </summary>
        /// <param name="value">SensorData</param>
        /// <param name="targetType">Default IValueConverter param. Not used in current implementation.</param>
        /// <param name="parameter">String containing which sensorvalue should be converted.</param>
        /// <param name="language">Default IValueConverter param. Not used in current implementation.</param>
        /// <returns>SensorValue string representation.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Data data = (Data)value;
            switch (parameter)
            {
                case "Temperature":
                    {
                        if (data != null)
                            return data.TemperatureValue.ToString("F2") + " °C";
                        return "";
                    }
                case "Sound":
                    {
                        if (data != null)
                            return $"{data.SoundValue} dB";
                        return "";
                    }
                case "TVOC":
                    {
                        if (data != null)
                            return data.TvocValue.ToString("F2") + " ppm";
                        return "";
                    }
                case "Humidity":
                    {
                        if (data != null)
                            return data.HumidityValue.ToString("F2") + " %";
                        return "";
                    }
                case "Brightness":
                    {
                        if (data != null)
                            return $"{data.BrightnessValue} lx";
                        return "";
                    }
                default: return "";
            }
        }

        /// <summary>
        /// Converting string to data. Not used in current implementation.
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
