using System;
using System.Numerics;
using System.Text;

namespace SimulatorProgram
{
    /// <summary>
    /// DTO for containing basic sensor values (Temperature, Humidity, etc.) with Timestamp
    /// </summary>
    public class SensorData
    {
        public long unixTime;
        public float temperatureValue;
        public float soundValue;
        public float tvocValue;
        public float humidityValue;
        public float brightnessValue;
        public override string ToString()
        {
            return $"\t{DateTimeOffset.FromUnixTimeSeconds(unixTime).ToString("yyyy/M/d HH:mm:ss")}\t{temperatureValue}°C, {soundValue} dB, {tvocValue} ppm, {humidityValue}%, {brightnessValue} lux";
        }
    }
}
