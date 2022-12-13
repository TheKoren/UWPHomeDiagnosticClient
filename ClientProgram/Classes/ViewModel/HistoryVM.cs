using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ClientProgram
{
    /// <summary>
    /// Singleton ViewModel for HistoryVM
    /// Used for the querry logic
    /// </summary>
    public class HistoryVM : ObservableCollection<Data>
    {
        /// <summary>
        /// Command for serializing data to json.
        /// </summary>
        public SaveValuesCommand SaveValuesCommand { get; set; }

        /// <summary>
        /// Singleton class for logging.
        /// </summary>
        public Info Info;

        /// <summary>
        /// Eventhandler if the FilterSwitch has changed.
        /// </summary>
        public event PropertyChangedEventHandler FilterSwitchStateChanged;
        /// <summary>
        /// Eventhandler if the data has changed.
        /// </summary>
        public event PropertyChangedEventHandler DateChanged;

        private ObservableCollection<Data> displayedValues;
        /// <summary>
        /// ObservableCollection for showing the displayedValues.
        /// </summary>
        public ObservableCollection<Data> DisplayedValues
        {
            get { return displayedValues; }
            set
            {
                if (displayedValues != value)
                {
                    displayedValues = value;
                }
            }
        }

        private DateTimeOffset? startDate;
        /// <summary>
        /// StartDate for filtering.
        /// </summary>
        public DateTimeOffset? StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    if (value == null)
                        startDate = null;
                    else
                        startDate = new DateTimeOffset(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, new TimeSpan(0));

                    DateChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(StartDate)));
                }
            }
        }

        private TimeSpan? startTime;

        /// <summary>
        /// StartTime for filtering.
        /// </summary>
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    DateChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartTime)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(StartTime)));
                }
            }
        }

        private DateTimeOffset? endDate;

        /// <summary>
        /// EndDate for filtering.
        /// </summary>
        public DateTimeOffset? EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    if (value == null)
                        endDate = null;
                    else
                        endDate = new DateTimeOffset(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, new TimeSpan(0));
                    
                    DateChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(EndDate)));
                }
            }
        }

        private TimeSpan? endTime;
        /// <summary>
        /// EndTime for filtering.
        /// </summary>
        public TimeSpan? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    DateChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndTime)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(EndTime)));
                }
            }
        }

        private bool filterSwitchState;
        /// <summary>
        /// Boolean which shows the current state of the switch.
        /// </summary>
        public bool FilterSwitchState
        {
            get { return filterSwitchState; }
            set
            {
                if (filterSwitchState != value)
                {
                    filterSwitchState = value;
                    FilterSwitchStateChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSwitchState)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(FilterSwitchState)));
                }
            }
        }

        private static HistoryVM instance = null;

        /// <summary>
        /// Getter for the singleton class.
        /// </summary>
        public static HistoryVM GetInstance
        {
            get => instance ?? (instance = new HistoryVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private HistoryVM()
        {
            SaveValuesCommand = new SaveValuesCommand();

            Info = Info.GetInstance;

            DisplayedValues = new ObservableCollection<Data>();
            DisplayedValues.CollectionChanged += DisplayedValues_CollectionChanged;

            DateChanged += RefreshDisplayedValues;
            StartDate = null;
            StartTime = null;
            EndDate = null;
            EndTime = null;

            FilterSwitchStateChanged += RefreshDisplayedValues;
            filterSwitchState = false;

            SensorValuesVM.GetInstance.Values.CollectionChanged += RefreshDisplayedValues;
        }

        /// <summary>
        /// Refreshes the DisplayedValues Collection based on what has happened on the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshDisplayedValues(object sender, object e)
        {
            // If the refresh was invoked by a UI property (FilterSwitch or Date change)
            if (e is PropertyChangedEventArgs)
            {
                // And this UI property was the FilterSwitch
                if (((PropertyChangedEventArgs)e).PropertyName == nameof(FilterSwitchState))
                {
                    // And it was switched ON
                    if (FilterSwitchState)
                    {
                        ExecuteQueryToDisplayedValues();
                    }
                    //And it was switched OFF
                    else
                    {
                        ResetDisplayedValuesToSensorValues();
                    }
                }
                // And this UI property was one of the Date/Time pickers
                else
                {
                    // And the switch was ON
                    if (FilterSwitchState)
                    {
                        ExecuteQueryToDisplayedValues();
                    }
                }
            }
            // If the refresh was invoked by a background change in the Values collection
            else if (e is NotifyCollectionChangedEventArgs)
            {
                // And the switch was OFF
                if (!FilterSwitchState)
                {
                    // We just Remove/Add the Old/New items
                    if (((NotifyCollectionChangedEventArgs)e).OldItems != null)
                    {
                        foreach (Data d in ((NotifyCollectionChangedEventArgs)e).OldItems)
                            DisplayedValues.Remove(d);
                    }
                    if (((NotifyCollectionChangedEventArgs)e).NewItems != null)
                    {
                        foreach (Data d in ((NotifyCollectionChangedEventArgs)e).NewItems)
                            DisplayedValues.Insert(0, d);
                    }
                }
                // And the switch was ON
                else
                {
                    // We only Remove/Add the relevant items
                    if (((NotifyCollectionChangedEventArgs)e).OldItems != null)
                    {
                        foreach (Data d in ((NotifyCollectionChangedEventArgs)e).OldItems)
                        {
                            if (DisplayedValues.Contains(d))
                                DisplayedValues.Remove(d);
                        }
                    }
                    if (((NotifyCollectionChangedEventArgs)e).NewItems != null)
                    {
                        foreach (Data d in ((NotifyCollectionChangedEventArgs)e).NewItems)
                        {
                            if (StartDate == null && EndDate == null)
                                DisplayedValues.Insert(0, d);
                            else if (StartDate != null && EndDate == null)
                            {
                                long start = ((DateTimeOffset)StartDate).ToUnixTimeSeconds();
                                if (StartTime != null)
                                    start += (long)(((TimeSpan)StartTime).TotalSeconds);

                                if (start <= d.UnixTime)
                                    DisplayedValues.Insert(0, d);
                            }
                            else if (StartDate == null && EndDate != null)
                            {
                                long end = ((DateTimeOffset)EndDate).ToUnixTimeSeconds();
                                if (EndTime != null)
                                    end += (long)(((TimeSpan)EndTime).TotalSeconds);

                                if (d.UnixTime <= end)
                                    DisplayedValues.Insert(0, d);
                            }
                            else
                            {
                                long start = ((DateTimeOffset)StartDate).ToUnixTimeSeconds();
                                if (StartTime != null)
                                    start += (long)(((TimeSpan)StartTime).TotalSeconds);

                                long end = ((DateTimeOffset)EndDate).ToUnixTimeSeconds();
                                if (EndTime != null)
                                    end += (long)(((TimeSpan)EndTime).TotalSeconds);

                                if (start <= d.UnixTime && d.UnixTime <= end)
                                    DisplayedValues.Insert(0, d);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when an item is added, removed, or moved, or the entire collection is refreshed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayedValues_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= DisplayedValue_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += DisplayedValue_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// Occurs when an item inside of the collection is modified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayedValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        /// <summary>
        /// This function implements a simple querry method using LINQ
        /// </summary>
        /// <param name="fromTimeStamp">UnixTimeStamp from when we want data</param>
        /// <param name="tillTimeStamp">UnixTimeStamp till when we want data</param>
        public ObservableCollection<Data> QueryDataFromHistory(long fromTimeStamp, long tillTimeStamp)
        {
            ObservableCollection<Data> history = SensorValuesVM.GetInstance.Values;
            var querry = from data in history
                             where data.UnixTime >= fromTimeStamp && data.UnixTime <= tillTimeStamp
                             orderby data.UnixTime ascending    
                             select data;
            return new ObservableCollection<Data>(querry);
        }
        /// <summary>
        /// This function implements a simple querry method using LINQ.
        /// This method will querry the all data, from/to the beginning/end timestamp.
        /// </summary>
        /// <param name="timeStamp">UnixTimeStamp, which represents the beginning/end of the data</param>
        /// <param name="from_to">Bool, which determines whether the timeStamp is the beginning or the end of the data</param>
        public ObservableCollection<Data> QueryDataFromHistory(long timeStamp, bool from_to)
        {
            ObservableCollection<Data> history = SensorValuesVM.GetInstance.Values;

            IOrderedEnumerable<Data> query;
            // timeStamp -> beginning
            if (from_to == false)
            {
                query = from data in history
                             where data.UnixTime >= timeStamp
                             orderby data.UnixTime ascending
                             select data;
            }
            // timeStamp -> end
            else // if (from_to == true)
            {
                query = from data in history
                             where data.UnixTime <= timeStamp
                             orderby data.UnixTime ascending
                             select data;
            }
            
            return new ObservableCollection<Data>(query);
        }

        /// <summary>
        /// This function sets and uses the previously implemented Query functions.
        /// </summary>
        public void ExecuteQueryToDisplayedValues()
        {
            // Query is unnecessary
            if (StartDate == null && EndDate == null)
                ResetDisplayedValuesToSensorValues();
            // From
            if (StartDate != null && EndDate == null)
            {
                long from = ((DateTimeOffset)StartDate).ToUnixTimeSeconds();
                if (StartTime != null)
                    from += (long)(((TimeSpan)StartTime).TotalSeconds);

                DisplayedValues.Clear();
                ObservableCollection<Data> QueryData = QueryDataFromHistory(from, false);
                foreach (Data d in QueryData)
                    DisplayedValues.Insert(0, d);
            }
            // To
            else if (StartDate == null && EndDate != null)
            {
                long to = ((DateTimeOffset)EndDate).ToUnixTimeSeconds();
                if (EndTime != null)
                    to += (long)(((TimeSpan)EndTime).TotalSeconds);

                DisplayedValues.Clear();
                ObservableCollection<Data> QueryData = QueryDataFromHistory(to, true);
                foreach (Data d in QueryData)
                    DisplayedValues.Insert(0, d);
            }
            // From - To
            else if (StartDate != null && EndDate != null)
            {
                long from = ((DateTimeOffset)StartDate).ToUnixTimeSeconds();
                if (StartTime != null)
                    from += (long)(((TimeSpan)StartTime).TotalSeconds);
                long to = ((DateTimeOffset)EndDate).ToUnixTimeSeconds();
                if (EndTime != null)
                    to += (long)(((TimeSpan)EndTime).TotalSeconds);

                DisplayedValues.Clear();
                ObservableCollection<Data> QueryData = QueryDataFromHistory(from, to);
                foreach (Data d in QueryData)
                    DisplayedValues.Insert(0, d);
            }
        }

        /// <summary>
        /// This function resets the DisplayedValues Collection to the content of the SensorValues.
        /// </summary>
        public void ResetDisplayedValuesToSensorValues()
        {
            DisplayedValues.Clear();
            foreach (Data d in SensorValuesVM.GetInstance.Values)
                DisplayedValues.Insert(0, d);
        }
    }
}
