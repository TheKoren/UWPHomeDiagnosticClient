namespace ClientProgram
{
    /// <summary>
    /// Model for managing the windowframes
    /// </summary>
    public class WindowFrameManager : ObservableObject
    {
        private int windowFrameState;
        /// <summary>
        /// Containing the page currently displayed.
        /// </summary>
        public int WindowFrameState
        {
            get => windowFrameState;
            set
            {
                if (windowFrameState != value)
                {
                    windowFrameState = value;
                    Notify(nameof(WindowFrameState));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state">Number that represents the type of pages.</param>
        public WindowFrameManager(int state)
        {
            WindowFrameState = state;
        }
    }
}
