using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace ClientProgram
{
    /// <summary>
    /// UI Page to control the room enviroment.
    /// </summary>
    public sealed partial class ControlPage : Page
    {
        private Info Info;
        private ControlVM ControlViewModel;
        private ClientCalls ClientCallsInstance;
        
        private bool canScroll = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ControlPage()
        {
            this.InitializeComponent();

            Info = Info.GetInstance;
            ControlViewModel = ControlVM.GetInstance;
            ClientCallsInstance = ClientCalls.GetInstance;

            Info.PropertyChanged += ScrollCheck;
            LogBox.TextChanged += ScrollToBottom;
            LogBox.Loaded += ScrollToBottom;
        }

        /// <summary>
        /// Checks if we are scrolled to the bottom of the logbox.
        /// </summary>
        /// <param name="sender">ControlPage itself.</param>
        /// <param name="e">Event argument.</param>
        private void ScrollCheck(object sender, PropertyChangedEventArgs e)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(LogBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer))
                    continue;

                ScrollViewer scrollViewer = (ScrollViewer)obj;
                if ((scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset) < 24)
                    canScroll = true;
                break;
            }
        }

        /// <summary>
        /// If the scrollcheck was successful it executes the scroll.
        /// </summary>
        /// <param name="sender">ControlPage itself.</param>
        /// <param name="e">Event argument.</param>
        private void ScrollToBottom(object sender, TextChangedEventArgs e)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(LogBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer))
                    continue;

                ScrollViewer scrollViewer = (ScrollViewer)obj;
                if (canScroll)
                    scrollViewer.ChangeView(0.0f, scrollViewer.ExtentHeight, 1.0f);
                canScroll = false;
                break;
            }
        }

        /// <summary>
        /// Scrolls to the bottom when the control page is loaded.
        /// </summary>
        /// <param name="sender">ControlPage itself.</param>
        /// <param name="e">Event argument.</param>
        private void ScrollToBottom(object sender, RoutedEventArgs e)
        {
            if (MainVM.GetInstance.WindowFrameState == 1)
            {
                var grid = (Grid)VisualTreeHelper.GetChild(LogBox, 0);
                for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
                {
                    object obj = VisualTreeHelper.GetChild(grid, i);
                    if (!(obj is ScrollViewer))
                        continue;

                    ScrollViewer scrollViewer = (ScrollViewer)obj;
                    scrollViewer.ChangeView(0.0f, scrollViewer.ExtentHeight, 1.0f);
                    break;
                }
            }
        }
    }
}
