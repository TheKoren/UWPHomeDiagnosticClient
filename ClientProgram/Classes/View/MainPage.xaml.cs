using Windows.UI.Xaml.Controls;


namespace ClientProgram
{
    /// <summary>
    /// MainPage containing all other elements.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Displays the software's current version.
        /// </summary>
        public string CurrentVersion;
        private Clock clock;

        // Objects to use x:Bind
        private MainVM MainViewModel = MainVM.GetInstance;
        private SensorValuesVM SensorValuesViewModel = SensorValuesVM.GetInstance;
        private FridgeVM FridgeViewModel = FridgeVM.GetInstance;
        private ControlVM ControlViewModel = ControlVM.GetInstance;
        private HistoryVM HistoryViewModel = HistoryVM.GetInstance;
        private ClientCalls ClientCallsInstance = ClientCalls.GetInstance;

        /// <summary>
        /// Constructor for Mainpage.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            CurrentVersion = "Version: v1.0";
            clock = new Clock();
        }
    }
}
