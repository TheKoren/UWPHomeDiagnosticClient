using System.ComponentModel;


namespace ClientProgram
{
    /// <summary>
    /// ViewModel for the Control.
    /// </summary>
    public class ControlVM : ObservableObject
    {
        /// <summary>
        /// Model for the control.
        /// </summary>
        public Control Model { get; internal set; }
        /// <summary>
        /// Command for saving the current data to a json.
        /// </summary>
        public SaveValuesCommand SaveValuesCommand { get; internal set; }

        /// <summary>
        /// Class for custom logging.
        /// </summary>
        public Info Info;
        private ClientCalls ClientCallsInstance = ClientCalls.GetInstance;

        /// <summary>
        /// Stores the status of the switch controlling the thermostat.
        /// </summary>
        public bool ThermostatSwitch
        {
            get => Model.ThermostatSwitch;
            set { Model.ThermostatSwitch = value; }
        }
        /// <summary>
        /// Stores the value of the thermostat slider.
        /// </summary>
        public float ThermostatSlider
        {
            get => Model.ThermostatSlider;
            set { Model.ThermostatSlider = value; }
        }

        /// <summary>
        /// Stores the status of the switch controlling the speakers.
        /// </summary>
        public bool SpeakersSwitch
        {
            get => Model.SpeakersSwitch;
            set { Model.SpeakersSwitch = value; }
        }
        /// <summary>
        /// Stores the value of the slider controlling the speakers.
        /// </summary>
        public float SpeakersSlider
        {
            get => Model.SpeakersSlider;
            set { Model.SpeakersSlider = value; }
        }

        /// <summary>
        /// Stores the status of the switch controlling the humidifer.
        /// </summary>
        public bool HumidifierSwitch
        {
            get => Model.HumidifierSwitch;
            set { Model.HumidifierSwitch = value; }
        }
        /// <summary>
        /// Stores the value of the slider controlling the humidifier.
        /// </summary>
        public float HumidifierSlider
        {
            get => Model.HumidifierSlider;
            set { Model.HumidifierSlider = value; }
        }

        /// <summary>
        /// Boolean to enable control for thermostatslider
        /// </summary>
        public bool Slider1Enabled { get => (ThermostatSwitch && ClientCallsInstance.IsConnected); }
        /// <summary>
        /// Boolean to enable control for speakerslider.
        /// </summary>
        public bool Slider2Enabled { get => (SpeakersSwitch && ClientCallsInstance.IsConnected); }
        /// <summary>
        /// Booleaon to enable control for humidiferslider.
        /// </summary>
        public bool Slider3Enabled { get => (HumidifierSwitch && ClientCallsInstance.IsConnected); }

        /// <summary>
        /// Stores the status of the switch controlling the lights.
        /// </summary>
        public bool LightsSwitch
        {
            get => Model.LightsSwitch;
            set { Model.LightsSwitch = value; }
        }

        private static ControlVM instance = null;
        /// <summary>
        /// Getter for the singleton object.
        /// </summary>
        public static ControlVM GetInstance
        {
            get => instance ?? (instance = new ControlVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private ControlVM()
        {
            Model = new Control();
            Model.PropertyChanged += Model_PropertyChanged;
            ClientCallsInstance.PropertyChanged += Model_PropertyChanged;

            SaveValuesCommand = new SaveValuesCommand();

            Info = Info.GetInstance;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Control.ThermostatSwitch))
            {
                Notify(nameof(ThermostatSwitch));
                Notify(nameof(Slider1Enabled));
            }
                
            else if (e.PropertyName == nameof(Control.ThermostatSlider))
                Notify(nameof(ThermostatSlider));
            else if (e.PropertyName == nameof(Control.SpeakersSwitch))
            {
                Notify(nameof(SpeakersSwitch));
                Notify(nameof(Slider2Enabled));
            }
            else if (e.PropertyName == nameof(Control.SpeakersSlider))
                Notify(nameof(SpeakersSlider));
            else if (e.PropertyName == nameof(Control.HumidifierSwitch))
            {
                Notify(nameof(HumidifierSwitch));
                Notify(nameof(Slider3Enabled));

            }
            else if (e.PropertyName == nameof(Control.HumidifierSlider))
                Notify(nameof(HumidifierSlider));
            else if (e.PropertyName == nameof(ClientCallsInstance.IsConnected))
            {
                Notify(nameof(Slider1Enabled));
                Notify(nameof(Slider2Enabled));
                Notify(nameof(Slider3Enabled));
            }
            else if (e.PropertyName == nameof(Control.LightsSwitch))
                Notify(nameof(LightsSwitch));
        }
    }
}
