using System.Diagnostics;


namespace ClientProgram
{
    /// <summary>
    /// Singleton class used for logging
    /// </summary>
    public class Info : ObservableObject
    {
        private string logBoxText;
        /// <summary>
        /// The content of the logbox.
        /// </summary>
        public string LogBoxText
        {
            get => logBoxText;
            set
            {
                if (logBoxText != value)
                {
                    logBoxText = value;
                    Notify(nameof(LogBoxText));
                }
            }
        }

        private static Info instance = null;

        /// <summary>
        /// Getter for the singleton instance.
        /// </summary>
        public static Info GetInstance
        {
            get => instance ?? (instance = new Info());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private Info()
        {
            LogBoxText = "";
        }

        /// <summary>
        /// Function for logging.
        /// </summary>
        /// <param name="message">String message to log.</param>
        public void Log(string message)
        {
            LogBoxText += (message + "\r\n");
            Debug.WriteLine(message);
        }
    }
}
