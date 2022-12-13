using System;
using Windows.UI.Xaml;


namespace ClientProgram
{
    /// <summary>
    /// Class for showing current time on the UI.
    /// </summary>
    public class Clock : ObservableObject
    {
        private DispatcherTimer Timer;

        private string timeText;
        /// <summary>
        /// Contains the exact time of the clock (Hour, Minute, Second).
        /// </summary>
        public string TimeText
        {
            get => timeText;
            internal set
            {
                timeText = value;
            }
        }

        private string dateText;
        /// <summary>
        /// Contains the date of the clock (Year, Month, Day).
        /// </summary>
        public string DateText
        {
            get => dateText;
            internal set
            {
                dateText = value;
            }
        }

        /// <summary>
        /// Constructor, which starts the Timer used for updating the Clocks current value.
        /// </summary>
        public Clock()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Clock_Tick;
            dateText = " : : ";
            timeText = " : : ";

            Timer.Start();
        }

        /// <summary>
        /// Every interval, this function is invoked.
        /// </summary>
        /// <param name="sender">Function invoker.</param>
        /// <param name="e">Provides data for the Elapsed event.</param>
        private void Clock_Tick(object sender, object e)
        {
            DateText = DateTime.Now.ToString("MMMM dd.");
            Notify(nameof(DateText));
            TimeText = DateTime.Now.ToString("HH : mm : ss");
            Notify(nameof(TimeText));
        }
    }
}
