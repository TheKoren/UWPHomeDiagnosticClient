using System.ComponentModel;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// ViewModel for the mainpage.
    /// </summary>
    public class MainVM : ObservableObject
    {
        /// <summary>
        /// Model for the FrameManager
        /// </summary>
        public WindowFrameManager Model { get; internal set; }

        private ClientCalls ClientCallsInstance;

        /// <summary>
        /// Command for connecting to the server.
        /// </summary>
        public ConnectCommand ConnectCommand { get; internal set; }
        /// <summary>
        /// Command for disconnecting from the server.
        /// </summary>
        public DisconnectCommand DisconnectCommand { get; internal set; }

        /// <summary>
        /// State of the frame.
        /// </summary>
        public int WindowFrameState
        {
            get => Model.WindowFrameState;
            set
            {
                Model.WindowFrameState = value;
            }
        }

        private ICommand currentCommand;
        /// <summary>
        /// Current command of the Connect/Disconnect button.
        /// </summary>
        public ICommand CurrentCommand
        {
            get => currentCommand;
            set
            {
                if (currentCommand != value)
                {
                    currentCommand = value;
                    Notify(nameof(CurrentCommand));
                }
            }
        }

        private string buttonContent;
        /// <summary>
        /// Current content of the Connect/Disconnect button.
        /// </summary>
        public string ButtonContent
        {
            get => buttonContent;
            set
            {
                if (buttonContent != value)
                {
                    buttonContent = value;
                    Notify(nameof(ButtonContent));
                }
            }
        }


        private static MainVM instance;
        /// <summary>
        /// Getter for the singleton instance.
        /// </summary>
        public static MainVM GetInstance
        {
            get => instance ?? (instance = new MainVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private MainVM()
        {
            Model = new WindowFrameManager(0);
            Model.PropertyChanged += Model_PropertyChanged;

            ClientCallsInstance = ClientCalls.GetInstance;

            ConnectCommand = new ConnectCommand();
            DisconnectCommand = new DisconnectCommand();

            CurrentCommand = ConnectCommand;
            ButtonContent = "Connect";
            ClientCallsInstance.PropertyChanged += UpdateConnectButton;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WindowFrameManager.WindowFrameState))
                Notify(nameof(WindowFrameState));
        }

        private void UpdateConnectButton(object sender, PropertyChangedEventArgs e)
        {
            if (ClientCallsInstance.IsConnected)
            {
                CurrentCommand = DisconnectCommand;
                ButtonContent = "Disconnect";
            }
            else
            {
                CurrentCommand = ConnectCommand;
                ButtonContent = "Connect";
            }
        }
    }
}
