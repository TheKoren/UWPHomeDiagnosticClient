using System;
using System.Collections.Generic;

namespace SimulatorProgram
{
    public enum ControlState { On, Off }

    /// <summary>
    /// Wrapper class for ambient properties that can be controlled
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Create and start an ambient property simulation
        /// </summary>
        /// <param name="property">Ambient property to be simulated</param>
        public Control(Ambient property)
        {
            Property = property;
            Property.MainHandler(false);
        }

        /// <summary>
        /// Current value of the simulated property
        /// </summary>
        public float Value
        {
            get { return Property.currentValue; }
        }

        /// <summary>
        /// State of control of the proprty (On/Off)
        /// </summary>
        public ControlState State
        {
            get { return Property.controlIsOn ? ControlState.On : ControlState.Off; }
        }
        private Ambient Property;

        /// <summary>
        /// Turns on applience and sets new target value
        /// </summary>
        /// <param name="val">Target value we desire to reach</param>
        public void SetNewValue(float val)
        {
            Property.controlIsOn = true;
            Property.desiredValue = val;
        }

        /// <summary>
        /// Turn off control of property
        /// </summary>
        public void TurnOff()
        {
            Property.controlIsOn = false;
        }

    }

    /// <summary>
    /// Wrapper class for properties that can't be controlled
    /// </summary>
    public class Uncontrolled
    {
        /// <summary>
        /// Create and start an ambient property simulation
        /// </summary>
        /// <param name="property">Ambient property to be simulated</param>
        public Uncontrolled(Ambient property)
        {
            Property = property;
            Property.MainHandler(false);
        }
        private Ambient Property;

        /// <summary>
        /// Current value of the simulated property
        /// </summary>
        public float Value {get => Property.currentValue;}
    }

    /// <summary>
    /// Class that handles the simulation of the full room
    /// All ambient property simulations are started
    /// </summary>
    class Simulation
    {
        public Control Temperature = new Control(new Temperature());
        public Control Humidity = new Control(new Humidity());
        public Control Brightness = new Control(new Brightness());
        public Control Sound = new Control(new Sound());
        public Uncontrolled Tvoc = new Uncontrolled(new Tvoc());

        /// <summary>
        /// Fills SensorData DTO class with current values
        /// </summary>
        /// <returns>New SensorData instance</returns>
        public SensorData ToSensorData()
        {
            return new SensorData()
            {
                unixTime = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                temperatureValue = Temperature.Value,
                tvocValue = Tvoc.Value,
                soundValue = Sound.Value,
                humidityValue = Humidity.Value,
                brightnessValue = Brightness.Value
            };
        }

        /// <summary>
        /// Applies new control values based on SendData DTO class contents
        /// The simulation will set these values as targets.
        /// </summary>
        /// <param name="data">SendData class containing control values</param>
        public void ApplyNewControls(SendData data)
        {
            List<Tuple<Tuple<float, bool>, Control>> controlList = new List<Tuple<Tuple<float, bool>, Control>>();
            controlList.Add(new Tuple<Tuple<float, bool>, Control>(new Tuple<float, bool>((float)data.ThermostatSlider, data.ThermostatSwitch), Temperature));
            controlList.Add(new Tuple<Tuple<float, bool>, Control>(new Tuple<float, bool>((float)data.HumidifierSlider, data.HumidifierSwitch), Humidity));
            controlList.Add(new Tuple<Tuple<float, bool>, Control>(new Tuple<float, bool>((float)data.SpeakersSlider, data.SpeakersSwitch), Sound));
            controlList.Add(new Tuple<Tuple<float, bool>, Control>(new Tuple<float, bool>(1, data.LightsSwitch), Brightness));

            foreach (var c in controlList)
            {
                if (!c.Item1.Item2)
                {
                    c.Item2.TurnOff();
                }
                else
                {
                    c.Item2.SetNewValue(c.Item1.Item1);
                }
            }
        }
    }
}
