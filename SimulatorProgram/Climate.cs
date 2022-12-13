using System;
using System.Threading.Tasks;

namespace SimulatorProgram
{
    /// <summary>
    /// Very basic time of day enum
    /// </summary>
    public enum TimeOfDay { Day, Night, Dusk, Dawn }

    /// <summary>
    /// Abstract base class for all Climate related classes.
    /// </summary>
    public abstract class Ambient
    {
        /// <summary>
        /// Used to store the value, if control is off
        /// </summary>
        protected float baseValue = 0;

        /// <summary>
        /// Difference of baseValue and currentValue
        /// </summary>
        protected float offsetValue = 0;

        /// <summary>
        /// The value we want to reach when controlling an ambient property
        /// </summary>
        public float desiredValue { get; set; }

        /// <summary>
        /// The current value of the ambient property (eg. current temperature)
        /// </summary>
        public float currentValue { get; set; }

        /// <summary>
        /// Set to true to control this ambient property
        /// </summary>
        public bool controlIsOn = false;
        protected abstract TimeSpan updateTime { get; }

        /// <summary>
        /// Infinite loop, updated values based on UpdateFields() method
        /// </summary>
        /// <param name="time">How often values are updated</param>
        /// <param name="exitCondition">If this condition is met, the task finishes. Set to false if it should not finish</param>
        /// <returns>Task</returns>
        public async Task MainHandler(bool exitCondition)
        {
            while (!exitCondition)
            {
                UpdateFields();
                await Task.Delay(updateTime);
            }
        }

        /// <summary>
        /// Define how the variables change in a single iteration
        /// </summary>
        protected abstract void UpdateFields();

        /// <summary>
        /// Get what part of the day is currently
        /// </summary>
        /// <returns> Current time of day in enum TimeOfDay</returns>
        public static TimeOfDay GetTimeOfDay()
        {
            int hour = DateTime.Now.Hour;
            if ((hour < 5) || (hour > 21))
            {
                return TimeOfDay.Night;
            }
            else if ((hour > 9) && (hour < 17))
            {
                return TimeOfDay.Day;
            }
            else if (hour <= 9)
            {
                return TimeOfDay.Dawn;
            }
            else
            {
                return TimeOfDay.Dusk;
            }
        }

        /// <summary>
        /// Get a random float value from given interval
        /// </summary>
        /// <param name="from">Lower bound of interval</param>
        /// <param name="to">Upper bound of interval</param>
        /// <returns>Random float value from given interval</returns>
        public static float RndFromInterval(float from, float to)
        {
            Random rnd = new Random();
            return (float)Math.Round((decimal)(((float)rnd.NextDouble()) * Math.Abs(to - from) + Math.Min(to, from)), 3);
        }
    }

    /// <summary>
    /// Class for simulating temperature
    /// Can be controlled
    /// </summary>
    class Temperature : Ambient
    {
        protected override TimeSpan updateTime { get => TimeSpan.FromSeconds(4); }
        protected override void UpdateFields()
        {
            float raw;
            TimeOfDay t = GetTimeOfDay();

            switch (t)
            {
                case TimeOfDay.Day:
                    raw = 20;
                    break;
                case TimeOfDay.Night:
                    raw = 16;
                    break;
                case TimeOfDay.Dusk:
                    raw = 17;
                    break;
                default:
                    raw = 18;
                    break;
            }
            baseValue = raw + RndFromInterval(-0.1f, 0.1f);
            if (controlIsOn)
            {
                if (currentValue > desiredValue)
                {
                    offsetValue -= 0.8f;
                }
                else
                {
                    offsetValue += 0.8f;
                }
            }
            else
            {
                if (baseValue > currentValue)
                {
                    offsetValue += 0.5f;
                }
                else
                {
                    offsetValue -= 0.5f;
                }
            }
            currentValue = baseValue + offsetValue;
        }
    }

    /// <summary>
    /// Class for simulating sound level
    /// Can be controlled
    /// </summary>
    class Sound : Ambient
    {
        protected override TimeSpan updateTime { get => TimeSpan.FromSeconds(1); }
        public Sound()
        {
            baseValue = 30.0f;
        }
        protected override void UpdateFields()
        {
            if (controlIsOn)
            {
                currentValue = desiredValue + baseValue;
            }
            else
            {
                currentValue = baseValue;
            }
            currentValue += RndFromInterval(-1.5f, 1.5f);
        }

    }

    /// <summary>
    /// Class for simulating Humidity
    /// Can be controlled
    /// </summary>
    class Humidity : Ambient
    {
        protected override TimeSpan updateTime { get => TimeSpan.FromSeconds(4); }
        protected override void UpdateFields()
        {
            float raw;
            TimeOfDay t = GetTimeOfDay();

            switch (t)
            {
                case TimeOfDay.Day:
                    raw = 55;
                    break;
                case TimeOfDay.Night:
                    raw = 60;
                    break;
                case TimeOfDay.Dusk:
                    raw = 65;
                    break;
                default:
                    raw = 60;
                    break;
            }
            baseValue = raw + RndFromInterval(-0.3f, 0.3f);
            if (controlIsOn)
            {
                if (currentValue > desiredValue)
                {
                    offsetValue -= 1.3f;
                }
                else
                {
                    offsetValue += 1.3f;
                }
            }
            else
            {
                if (baseValue > currentValue)
                {
                    offsetValue += 0.9f;
                }
                else
                {
                    offsetValue -= 0.9f;
                }
            }
            currentValue = baseValue + offsetValue;
        }
    }

    /// <summary>
    /// Class for simulating brightness
    /// Can be controlled
    /// </summary>
    class Brightness : Ambient
    {
        private const float maxBrightness = 300;
        private const float lampBrightness = 150;
        protected override TimeSpan updateTime { get => TimeSpan.FromSeconds(1); }
        protected override void UpdateFields()
        {
            TimeOfDay t = GetTimeOfDay();

            switch (t)
            {
                case TimeOfDay.Day:
                    baseValue = 280;
                    break;
                case TimeOfDay.Night:
                    baseValue = 20;
                    break;
                case TimeOfDay.Dusk:
                    baseValue = 80;
                    break;
                default:
                    baseValue = 120;
                    break;
            }
            offsetValue = controlIsOn ? lampBrightness : 0;
            currentValue = Math.Min(baseValue + offsetValue, maxBrightness) + RndFromInterval(-5, 5);
        }
    }

    /// <summary>
    /// Class for simulating TVOC
    /// Can't be controlled
    /// </summary>
    class Tvoc : Ambient
    {
        protected override TimeSpan updateTime { get => TimeSpan.FromSeconds(12); }
        private new float desiredValue { get; set; }
        private int counter = 0;
        public Tvoc()
        {
            baseValue = 200.0f;
        }
        protected override void UpdateFields()
        {
            if (counter % 3 == 0)
            {
                desiredValue = RndFromInterval(100, 400);
            }
            offsetValue += (desiredValue > currentValue) ? RndFromInterval(10, 40) : -RndFromInterval(10, 40);
            currentValue = baseValue + offsetValue;
        }
    }
}
