using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorProgram
{
    class Program
    {
        private static HttpListener listener;
        private static List<Item> items;
        private static Simulation simulation;

        private static readonly string BaseUri = "http://localhost:8000/";
        private static readonly string Signal = "signal/";
        private static readonly string Data = "data/";
        private static readonly string Fridge = "fridge/";
        private static readonly string SignalUri = string.Concat(BaseUri, Signal);
        private static readonly string DataUri = string.Concat(BaseUri, Data);
        private static readonly string FridgeUri = string.Concat(BaseUri, Fridge);

        static async Task Main()
        {
            listener = new HttpListener();
            items = new List<Item>();
            simulation = new Simulation();

            items.Add(new Item("Eggs", 4));
            items.Add(new Item("Bread", 1));

            listener.Prefixes.Add(SignalUri);
            listener.Prefixes.Add(DataUri);
            listener.Prefixes.Add(FridgeUri);

            listener.Start();
            Console.WriteLine("Listening to ClientProgram calls...");
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                try
                {
                    await HandlerMethodAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            listener.Close();
            Console.WriteLine("Stopped listening");
            Console.ReadKey();
        }

        private static async Task HandlerMethodAsync(HttpListenerContext context)
        {
            HttpListenerRequest req = context.Request;
            HttpListenerResponse resp = context.Response;
            Console.WriteLine($"URL: {req.Url} \t{req.HttpMethod}");
            if (req.Url.ToString().Equals(DataUri))
            {
                if (req.HttpMethod.Equals("POST"))
                {
                    await HandleControlPostAsync(req, resp);
                }
                else
                {
                    await HandleValuesGetAsync(req, resp);
                }
            }
            else if(req.Url.ToString().Equals(FridgeUri))
            {
                if(req.HttpMethod.Equals("POST"))
                {
                    await HandleFridgePostAsync(req, resp);
                }
                else if(req.HttpMethod.Equals("GET"))
                {
                    await HandleFridgeGetAsync(req, resp);
                }
                else
                {
                    await HandleFridgePutAsync(req, resp);
                }
            }
            else
            {
                await HandleSignalAsync(req, resp);
            }
        }

        private static async Task HandleFridgeGetAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string jsonString = JsonConvert.SerializeObject(items);

            await BuildResponse(resp, req.ContentEncoding, jsonString);
        }

        private static async Task HandleFridgePutAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string reqcontent = await GetStringContentAsync(req);

            JObject json = JObject.Parse(reqcontent);
            Item updatedItem = json.ToObject<Item>();

            if(updatedItem.Amount == 0)
            {
                items = items.Where(x => x.Name != updatedItem.Name).ToList();
            }
            else
            {
                items.Where(x => x.Name == updatedItem.Name).ToList().ForEach(i => i.Amount = updatedItem.Amount);
            }

            await BuildResponse(resp, req.ContentEncoding, $"Updated item name: \t {updatedItem.Name}");
        }

        private static async Task HandleFridgePostAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string reqcontent = await GetStringContentAsync(req);

            JObject json = JObject.Parse(reqcontent);
            Item newItem = json.ToObject<Item>();
            items.Add(newItem);

            await BuildResponse(resp, req.ContentEncoding, $"New item name: \t {newItem.Name}");
        }

        private static async Task<string> GetStringContentAsync(HttpListenerRequest req)
        {
            string result = "";
            using (var bodyStream = req.InputStream)
            {
                var encoding = req.ContentEncoding;
                using (var streamReader = new StreamReader(bodyStream, encoding))
                {
                    result = await streamReader.ReadToEndAsync();
                }
            }
            return result;
        }

        private static async Task BuildResponse(HttpListenerResponse resp, Encoding encoding, string content)
        {
            resp.StatusCode = 200;
            byte[] buffer = encoding.GetBytes(content);
            resp.ContentLength64 = buffer.Length;

            await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            resp.OutputStream.Close();
        }

        private static async Task HandleSignalAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string reqContent = await GetStringContentAsync(req);

            Console.WriteLine("Signal received.");

            await BuildResponse(resp, req.ContentEncoding, $"Signal content:\t {reqContent}");
        }

        private static async Task HandleControlPostAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string reqcontent = await GetStringContentAsync(req);

            JObject json = JObject.Parse(reqcontent);
            SendData control = json.ToObject<SendData>();
            simulation.ApplyNewControls(control);
            await BuildResponse(resp, req.ContentEncoding, "Control received");
        }

        private static async Task HandleValuesGetAsync(HttpListenerRequest req, HttpListenerResponse resp)
        {
            SensorData data = simulation.ToSensorData();
            Console.WriteLine($"Sending data: {data.ToString()}");
            string jsonString = JsonConvert.SerializeObject(data);

            await BuildResponse(resp, req.ContentEncoding, jsonString);
        }
    }
}