using Windows.UI.Xaml.Controls;


namespace ClientProgram
{
    /// <summary>
    /// Page where we can examine old data.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        private SensorValuesVM SensorValuesViewModel;
        private HistoryVM HistoryViewModel;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HistoryPage()
        {
            this.InitializeComponent();

            SensorValuesViewModel = SensorValuesVM.GetInstance;
            HistoryViewModel = HistoryVM.GetInstance;
        }
    }
}
