using System.Linq;
using Windows.UI.Xaml.Controls;


namespace ClientProgram
{
    /// <summary>
    /// Page to display the graph.
    /// </summary>
    public sealed partial class GraphPage : Page
    {
        private GraphVM graphViewModel;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GraphPage()
        {
            this.InitializeComponent();
            graphViewModel = GraphVM.GetInstance;
        }

        /// <summary>
        /// Event handler for when the user changes the selected graph
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments of selection changed event</param>
        private void GraphSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                graphViewModel.CurrentSelection = e.AddedItems.First().ToString();
            }
            graphViewModel.LoadSensorValues();
        }
    }
}
