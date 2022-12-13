namespace ClientProgram
{
    /// <summary>
    /// Model for controlling the embedded system.
    /// </summary>
    public class Control : ObservableObject
    {
        private bool thermostatSwitch;
        /// <summary>
        /// Stores the status of the switch controlling the thermostat.
        /// </summary>
        public bool ThermostatSwitch
        {
            get => thermostatSwitch;
            set
            {
                if (thermostatSwitch != value)
                {
                    thermostatSwitch = value;
                    Notify(nameof(ThermostatSwitch));
                }
            }
        }
        private float thermostatSlider;
        /// <summary>
        /// Stores the value of the thermostat slider.
        /// </summary>
        public float ThermostatSlider
        {
            get => thermostatSlider;
            set
            {
                if (thermostatSlider != value)
                {
                    thermostatSlider = value;
                    Notify(nameof(ThermostatSlider));
                }
            }
        }


        private bool speakersSwitch;
        /// <summary>
        /// Stores the status of the switch controlling the speakers.
        /// </summary>
        public bool SpeakersSwitch
        {
            get => speakersSwitch;
            set
            {
                if (speakersSwitch != value)
                {
                    speakersSwitch = value;
                    Notify(nameof(SpeakersSwitch));
                }
            }
        }
        private float speakersSlider;
        /// <summary>
        /// Stores the value of the slider controlling the speakers.
        /// </summary>
        public float SpeakersSlider
        {
            get => speakersSlider;
            set
            {
                if (speakersSlider != value)
                {
                    speakersSlider = value;
                    Notify(nameof(SpeakersSlider));
                }
            }
        }


        private bool humidifierSwitch;
        /// <summary>
        /// Stores the status of the switch controlling the humidifer.
        /// </summary>
        public bool HumidifierSwitch
        {
            get => humidifierSwitch;
            set
            {
                if (humidifierSwitch != value)
                {
                    humidifierSwitch = value;
                    Notify(nameof(HumidifierSwitch));
                }
            }
        }
        private float humidifierSlider;
        /// <summary>
        /// Stores the value of the slider controlling the humidifier.
        /// </summary>
        public float HumidifierSlider
        {
            get => humidifierSlider;
            set
            {
                if (humidifierSlider != value)
                {
                    humidifierSlider = value;
                    Notify(nameof(HumidifierSlider));
                }
            }
        }


        private bool lightsSwitch;
        /// <summary>
        /// Stores the status of the switch controlling the lights.
        /// </summary>
        public bool LightsSwitch
        {
            get => lightsSwitch;
            set
            {
                if (lightsSwitch != value)
                {
                    lightsSwitch = value;
                    Notify(nameof(LightsSwitch));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Control()
        {
            ThermostatSwitch = false;
            ThermostatSlider = 25;

            SpeakersSwitch = false;
            SpeakersSlider = 0;

            HumidifierSwitch = false;
            humidifierSlider = 40;

            lightsSwitch = false;
        }
    }
}
