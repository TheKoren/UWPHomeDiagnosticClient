using System;
using System.ComponentModel;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// This class implements ICommand. Used for connecting to the remote simulator.
    /// </summary>
    public class ConnectCommand : ICommand
    {
        private Info Info;
        private ClientCalls ClientCallsInstance;

        /// <summary>
        /// EventHandler for command changing.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand()
        {
            Info = Info.GetInstance;
            ClientCallsInstance = ClientCalls.GetInstance;
            ClientCallsInstance.PropertyChanged += IsConnected_PropertyChanged;
        }

        private void IsConnected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClientCalls.GetInstance.IsConnected))
            CanExecuteChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Commands can only be executed, if they fulfil some conditions.
        /// </summary>
        /// <param name="parameter">Parameter object.</param>
        /// <returns>Returns true, if the command can be executed. Otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            return !(ClientCallsInstance.IsConnected);
        }

        /// <summary>
        /// On Connect button click, this function is invoked.
        /// </summary>
        /// <param name="parameter">Paramter object</param>
        public void Execute(object parameter)
        {
            ClientCallsInstance.Connect();
            if (ClientCallsInstance.IsConnected)
            {
                Info.Log("[INFO] Connected to simulator.");
            }
        }
    }
}
