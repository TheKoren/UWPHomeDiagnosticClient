using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


namespace ClientProgram
{
    /// <summary>
    /// ViewModel for SensorValues.
    /// </summary>
    public class SensorValuesVM : ObservableCollection<Data>
    {
        /// <summary>
        /// Model for SensorValue Data
        /// </summary>
        public Data Model { get; internal set; }

        private Info Info;

        private Data lastData;
        /// <summary>
        /// Variable containing the latest data.
        /// </summary>
        public Data LastData
        {
            get => lastData;
            set
            {
                if (lastData != value)
                {
                    lastData = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(LastData)));
                }
            }
        }

        private ObservableCollection<Data> values;
        /// <summary>
        /// ObservableCollection for SensorValues.
        /// </summary>
        public ObservableCollection<Data> Values
        {
            get => values;
            set
            {
                if (values != value)
                {
                    values = value;
                }
            }
        }

        private static SensorValuesVM instance;

        /// <summary>
        /// Getter for singleton instance.
        /// </summary>
        public static SensorValuesVM GetInstance
        {
            get => instance ?? (instance = new SensorValuesVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private SensorValuesVM()
        {
            Info = Info.GetInstance;
            Values = new ObservableCollection<Data>();
            Values.CollectionChanged += Values_CollectionChanged;
        }

        private void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= Value_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += Value_PropertyChanged;
                }
            }
            LastData = Values.Last();
        }

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
    }
}
