using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientProgram
{
    /// <summary>
    /// Base class for INPC, Notify using CallerMemberName.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Unified propertychanges event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Easy to use function for change notification.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void Notify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
