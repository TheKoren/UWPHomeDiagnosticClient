using System;

namespace ClientProgram
{
    /// <summary>
    /// Class for containing basic sensor values (Temperature, Humidity, etc.) with Timestamp.
    /// </summary>
    public class Data : ObservableObject
    {
        private long unixTime;
        /// <summary>
        /// Timestamp of the measured values.
        /// </summary>
        public long UnixTime
        {
            get => unixTime;
            set
            {
                unixTime = value;
                Notify(nameof(UnixTime));
            }
        }

        private float temperatureValue;
        /// <summary>
        /// The temperature value of the current measurement.
        /// </summary>
        public float TemperatureValue
        {
            get => temperatureValue;
            set
            {
                temperatureValue = value;
                Notify(nameof(TemperatureValue));
            }
        }

        private float soundValue;
        /// <summary>
        /// The loudness value of the current measurement.
        /// </summary>
        public float SoundValue
        {
            get => soundValue;
            set
            {
                soundValue = value;
                Notify(nameof(SoundValue));
            }
        }

        private float tvocValue;
        /// <summary>
        /// The air quality value of the current measurement.
        /// </summary>
        public float TvocValue
        {
            get => tvocValue;
            set
            {
                tvocValue = value;
                Notify(nameof(TvocValue));
            }
        }

        private float humidityValue;
        /// <summary>
        /// The humidity value of the current measurement.
        /// </summary>
        public float HumidityValue
        {
            get => humidityValue;
            set
            {
                humidityValue = value;
                Notify(nameof(HumidityValue));
            }
        }

        private float brightnessValue;
        /// <summary>
        /// The brightness value of the current measurement.
        /// </summary>
        public float BrightnessValue
        {
            get => brightnessValue;
            set
            {
                brightnessValue = value;
                Notify(nameof(BrightnessValue));
            }
        }
    }
}
