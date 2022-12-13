using Windows.UI.Xaml.Controls;


namespace ClientProgram
{
    /// <summary>
    /// Page to display the room itself with latest values.
    /// </summary>
    public sealed partial class RoomPage : Page
    {
        private SensorValuesVM SensorValuesViewModel;
        public FridgeVM FridgeViewModel { get; set; }
        public ClientCalls ClientCallsInstance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RoomPage()
        {
            this.InitializeComponent();
            DataContext=this;

            SensorValuesViewModel = SensorValuesVM.GetInstance;
            FridgeViewModel = FridgeVM.GetInstance;
            ClientCallsInstance = ClientCalls.GetInstance;
        }
    }
}
