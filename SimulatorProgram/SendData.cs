namespace SimulatorProgram
{
    /// <summary>
    /// DTO object containing basic data coming from the UI (state of switches, buttons, etc.)
    /// </summary>
    public class SendData
    {
        public bool ThermostatSwitch;
        public double ThermostatSlider;
        public bool SpeakersSwitch;
        public double SpeakersSlider;
        public bool HumidifierSwitch;
        public double HumidifierSlider;
        public bool LightsSwitch;
    }
}
