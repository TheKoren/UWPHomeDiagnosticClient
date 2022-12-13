using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;
using System.Reflection;

namespace ClientProgram
{
    /// <summary>
    /// Singleton class for Graph View Model.
    /// Call GetInstance to have access to this class.
    /// </summary>
    public class GraphVM : ObservableObject
    {
        /// <summary>
        /// Instance of PlotModel, binds to a PlotView on GUI.
        /// </summary>
        public ViewResolvingPlotModel PlotModel { get; private set; } = new ViewResolvingPlotModel
        {
            Title = "Live ambient values",
            PlotAreaBorderColor = OxyColors.Transparent,
            PlotAreaBackground = OxyColors.Transparent,
            TitleColor = OxyColors.White,
            LegendTitleColor = OxyColors.White,
            LegendTextColor = OxyColors.White,
            Axes =
            {
                new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    AxislineColor = OxyColors.White,
                    TicklineColor = OxyColors.White,
                    TextColor = OxyColors.White,
                    TitleColor = OxyColors.White,
                    MajorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColors.White,
                    MajorGridlineThickness = 0.5,
                    MinorGridlineStyle = LineStyle.Dot,
                    MinorGridlineColor = OxyColors.White,
                    MinorGridlineThickness = 0.25
                },
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    AxislineColor = OxyColors.White,
                    TicklineColor = OxyColors.White,
                    TextColor = OxyColors.White,
                    TitleColor = OxyColors.White,
                    MajorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColors.White,
                    MajorGridlineThickness = 0.5,
                    MinorGridlineStyle = LineStyle.Dot,
                    MinorGridlineColor = OxyColors.White,
                    MinorGridlineThickness = 0.25
                },
            },
            Series =
            {
                new LineSeries
                {
                    Title = "LineSeries",
                    MarkerType = MarkerType.Diamond,
                }
            }
        };

        private static GraphVM instance = null;
        
        /// <summary>
        /// Returns a reference to the single instance, or creates it.
        /// </summary>
        public static GraphVM GetInstance
        {
            get => instance ?? (instance = new GraphVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private GraphVM()
        {
            SensorValuesVM.GetInstance.Values.CollectionChanged += UpdateGraph;
            Ambients = new List<string>()
            {
                "Temperature",
                "Humidity",
                "TVOC",
                "Brightness",
                "Sound",
            };
            CurrentSelection = "Temperature";
            EarliestShowingOffset = new TimeSpan(0, 2, 0);
        }

        /// <summary>
        /// Wrapper for <see cref="LoadSensorValues()"/> that can be subscribed
        /// collection changed events
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Collection changed event arguments.</param>
        private void UpdateGraph(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadSensorValues();
        }

        /// <summary>
        /// Updates the currently viewed graph.
        /// Call this function when you think something related to the graph changed.
        /// </summary>
        public void LoadSensorValues()
        {
            PlotModel.Series.Clear();
            LineSeries ser = new LineSeries()
            {
                Title = CurrentSelection,
                MarkerType = MarkerType.Circle,
                MarkerResolution = 5,
                MarkerSize = 5,
                Color = OxyColors.White,
                TextColor = OxyColors.White,
                LineStyle = LineStyle.Solid,
                BrokenLineThickness = 1,
                StrokeThickness = 1,
            };
            PlotModel.Title = $"Live {CurrentSelection} values";

            switch (CurrentSelection)
            {
                case "Temperature":
                    {
                        foreach (var val in SensorValuesVM.GetInstance.Values.Reverse())
                        {
                            if (DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime < DateTimeOffset.Now.Add(-EarliestShowingOffset))
                            {
                                break;
                            }
                            ser.Points.Add(DateTimeAxis.CreateDataPoint(DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime, val.TemperatureValue));
                        }
                        ser.MarkerFill = OxyColors.Red;
                        PlotModel.Axes[1].Minimum = 15f;
                        PlotModel.Axes[1].Maximum = 45f;
                        break;
                    }
                case "Humidity":
                    {
                        foreach (var val in SensorValuesVM.GetInstance.Values.Reverse())
                        {
                            if (DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime < DateTimeOffset.Now.Add(-EarliestShowingOffset))
                            {
                                continue;
                            }
                            ser.Points.Add(DateTimeAxis.CreateDataPoint(DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime, val.HumidityValue));
                        }
                        ser.MarkerFill = OxyColors.Blue;
                        PlotModel.Axes[1].Minimum = 0f;
                        PlotModel.Axes[1].Maximum = 100f;
                        break;
                    }
                case "TVOC":
                    {
                        foreach (var val in SensorValuesVM.GetInstance.Values.Reverse())
                        {
                            if (DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime < DateTimeOffset.Now.Add(-EarliestShowingOffset))
                            {
                                break;
                            }
                            ser.Points.Add(DateTimeAxis.CreateDataPoint(DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime, val.TvocValue));
                        }
                        ser.MarkerFill = OxyColors.Green;
                        PlotModel.Axes[1].Minimum = 50f;
                        PlotModel.Axes[1].Maximum = 450f;
                        break;
                    }
                case "Brightness":
                    {
                        foreach (var val in SensorValuesVM.GetInstance.Values.Reverse())
                        {
                            if (DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime < DateTimeOffset.Now.Add(-EarliestShowingOffset))
                            {
                                break;
                            }
                            ser.Points.Add(DateTimeAxis.CreateDataPoint(DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime, val.BrightnessValue));
                        }
                        ser.MarkerFill = OxyColors.Yellow;
                        PlotModel.Axes[1].Minimum = 0f;
                        PlotModel.Axes[1].Maximum = 320f;
                        break;
                    }
                case "Sound":
                    {
                        foreach (var val in SensorValuesVM.GetInstance.Values.Reverse())
                        {
                            if (DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime < DateTimeOffset.Now.Add(-EarliestShowingOffset))
                            {
                                break;
                            }
                            ser.Points.Add(DateTimeAxis.CreateDataPoint(DateTimeOffset.FromUnixTimeSeconds(val.UnixTime).DateTime, val.SoundValue));
                        }
                        ser.MarkerFill = OxyColors.LightBlue;
                        PlotModel.Axes[1].Minimum = 0f;
                        PlotModel.Axes[1].Maximum = 150f;
                        break;
                    }
                default:
                    {
                        Debug.WriteLine($"CurrentSelection: {CurrentSelection}");
                        throw new Exception("Failed out of graph selection");
                    }
            }
            PlotModel.Series.Add(ser);
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// List containing Ambients we measure.
        /// Used for Combobox data.
        /// </summary>
        public IList<string> Ambients { get; set; }

        /// <summary>
        /// Current selection of Combobox.
        /// </summary>
        public string CurrentSelection { get; set; }

        /// <summary>
        /// The earliest displayed data on the graph will have been received more recently than this offset.
        /// The default is 2 minutes.
        /// </summary>
        public TimeSpan EarliestShowingOffset { get; set; }
    }

    /// <summary>
    /// Use this sub implementation of the PlotModel if the view will be declared using data template.
    /// Because views will be automatically generated, and new view will be different this causes current version to throw an error.
    /// </summary>
    public class ViewResolvingPlotModel : PlotModel, IPlotModel
    {
        private static readonly Type BaseType = typeof(ViewResolvingPlotModel).BaseType;
        private static readonly MethodInfo BaseAttachMethod = BaseType
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(methodInfo => methodInfo.IsFinal && methodInfo.IsPrivate)
            .FirstOrDefault(methodInfo => methodInfo.Name.EndsWith(nameof(IPlotModel.AttachPlotView)));

        void IPlotModel.AttachPlotView(IPlotView plotView)
        {
            //because of issue https://github.com/oxyplot/oxyplot/issues/497 
            //only one view can ever be attached to one plotmodel
            //we have to force detach previous view and then attach new one
            if (plotView != null && PlotView != null && !Equals(plotView, PlotView))
            {
                BaseAttachMethod.Invoke(this, new object[] { null });
                BaseAttachMethod.Invoke(this, new object[] { plotView });
            }
            else
            {
                BaseAttachMethod.Invoke(this, new object[] { plotView });
            }
        }
    }
}