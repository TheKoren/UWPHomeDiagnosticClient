using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Timers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Xaml;

namespace ClientProgram
{
    /// <summary>
    /// This class holds the basic function for communicating with the simulator.
    /// </summary>
    public class ClientCalls : ObservableObject
    {
        private readonly HttpClient client;
        private DispatcherTimer valueTimer;
        private Info Info;
        private Timer controlTimer;
        private bool isConnected;

        private readonly string BaseUri;
        private readonly string Signal;
        private readonly string Data;
        private readonly string Fridge;
        private readonly string SignalUri;
        private readonly string DataUri;
        private readonly string FridgeUri;

        /// <summary>
        /// Value storing the information if the client is connected to the simulator.
        /// </summary>
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    Notify(nameof(IsConnected));
                }
            }
        }

        private static ClientCalls instance = null;
        /// <summary>
        /// Gets the instance of the ClientCalls singleton class.
        /// </summary>
        public static ClientCalls GetInstance
        {
            get => instance ?? (instance = new ClientCalls());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private ClientCalls()
        {
            Info = Info.GetInstance;
            client = new HttpClient();
            IsConnected = false;

            BaseUri = "http://localhost:8000/";
            Signal = "signal/";
            Data = "data/";
            Fridge = "fridge/";
            SignalUri = string.Concat(BaseUri, Signal);
            DataUri = string.Concat(BaseUri, Data);
            FridgeUri = string.Concat(BaseUri, Fridge);
        }

        /// <summary>
        /// Sends connection request to the simulator.
        /// </summary>
        async public void Connect()
        {
            if(!IsConnected)
            {
                client.DefaultRequestHeaders.ConnectionClose = true;
                var stringres = await SignalAsync("Connect");
                if (stringres != null)
                {
                    valueTimer = new DispatcherTimer();
                    valueTimer.Interval = new TimeSpan(0, 0, 4);
                    controlTimer = new Timer(5000);
                    valueTimer.Tick += UpdateValue;
                    valueTimer.Start();
                    controlTimer.Elapsed += ControlUpdate;
                    controlTimer.Start();
                    IsConnected = true;
                    List<Item> fridgeContent = await GetItemsListAsync();
                    FridgeVM.GetInstance.Items.Clear();
                    foreach (Item item in fridgeContent)
                    {
                        FridgeVM.GetInstance.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Sends disconnection request to the simulator.
        /// </summary>
        async public void Disconnect()
        {
            if (IsConnected)
            {
                client.DefaultRequestHeaders.ConnectionClose = true;
                var stringres = await SignalAsync("Disconnect");
                if (stringres != null)
                {
                    valueTimer.Stop();
                    controlTimer.Stop();
                    IsConnected = false;
                }
            }
        }

        /// <summary>
        /// Every interval, this function gets called, to update the sensor values stored in the client.
        /// </summary>
        /// <param name="sender">Function invoker</param>
        /// <param name="e">Provides data for the Elapsed event.</param>
        async void UpdateValue(object sender, object e)
        {
            if(!IsConnected)
            {
                return;
            }
            Info.Log($"[INFO] UpdateValue method called. IsConnected: {IsConnected}");
            var data = await GetDataAsync();
            if (data != null)
            {
                if (SensorValuesVM.GetInstance.Values.Contains(data) == false)
                {
                    SensorValuesVM.GetInstance.Values.Add(data);
                }
            }
            else
            {
                valueTimer.Stop();
            }            
        }

        /// <summary>
        /// Every interval, this function gets called, to update the controls on the simulator.
        /// </summary>
        /// <param name="sender">Function invoker.</param>
        /// <param name="e">Provides data for the Elapsed event.</param>
        async void ControlUpdate(object sender, ElapsedEventArgs e)
        {
            if (!IsConnected)
            {
                return;
            }
            Debug.WriteLine($"[INFO] ControlUpdate method called. IsConnected: {IsConnected}");
            var stringres = await PostDataAsync(ControlVM.GetInstance.Model);
            if(stringres == null)
            {
                controlTimer.Stop();
            }
        }

        /// <summary>
        /// Function to connect to the simulator.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A string containing information from Post method.</returns>
        async Task<string> SignalAsync(string message)
        {
            string stringres = null;
            try
            {
                var content = new StringContent(message);
                var result = await client.PostAsync(SignalUri, content);
                result.EnsureSuccessStatusCode();
                stringres = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"[POST] Connecting to simulator result: {stringres}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                isConnected = false;
            }
            return stringres;

        }

        /// <summary>
        /// Function to get the sensor values from the simulator.
        /// </summary>
        /// <returns>Data object containing the updated sensor values.</returns>
        async Task<Data> GetDataAsync()
        {
            Data data = null;
            try
            {
                var result = await client.GetAsync(DataUri);
                string stringres = await result.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<Data>(stringres);
                Info.Log($"[GET] Reading sensor values from Simulator: {JObject.Parse(stringres)}");
            }
            catch (HttpRequestException ex)
            {
                Info.Log(ex.Message);
                IsConnected = false;
            }
            return data;

        }

        /// <summary>
        /// Function to post data coming from the UI. Sends periodically.
        /// </summary>
        /// <param name="data">Object containing control parameters.</param>
        /// <returns>A string containing information from Post method.</returns>
        async Task<string> PostDataAsync(Control data)
        {
            string stringres = null;
            try
            {
                string jsonString = JsonConvert.SerializeObject(data);
                StringContent content = new StringContent(jsonString);
                var result = await client.PostAsync(DataUri, content);
                stringres = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"[POST] Sending UI data to simulator: {stringres}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                IsConnected = false;
            }
            return stringres;
        }

        /// <summary>
        /// Function to post new item from the UI to the simulator.
        /// </summary>
        /// <param name="item">New item made on the UI.</param>
        /// <returns>A string containing information from Post method.</returns>
        public async Task<string> PostItemAsync(Item item)
        {
            string stringres = null;
            try
            {
                string jsonString = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(jsonString);
                var result = await client.PostAsync(FridgeUri, content);
                stringres = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"[POST] Sending new fridge item to simulator: {stringres}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                IsConnected = false;
            }
            return stringres;
        }

        /// <summary>
        /// Function to update an item from the UI in the simulator.
        /// </summary>
        /// <param name="item">Updated item object.</param>
        /// <returns>A string containing information from Put method.</returns>
        public async Task<string> PutItemAsync(Item item)
        {
            string stringres = null;
            try
            {
                string jsonString = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(jsonString);
                var result = await client.PutAsync(FridgeUri, content);
                stringres = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"[PUT] Sending updated fridge item to simulator: {stringres}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                IsConnected = false;
            }
            return stringres;
        }

        /// <summary>
        /// Function to get all Items from the UI.
        /// </summary>
        /// <returns>List of Item objects, containing all of the fridge contents.</returns>
        async Task<List<Item>> GetItemsListAsync()
        {
            List<Item> items = null;
            try
            {
                var result = await client.GetAsync(FridgeUri);
                string stringres = await result.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<List<Item>>(stringres);
                Debug.WriteLine($"[GET] Reading fridge contents from simulator: {JArray.Parse(stringres)}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                IsConnected = false;
            }
            return items;
        }
    }
}
