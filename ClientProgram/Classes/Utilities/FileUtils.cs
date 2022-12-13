using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClientProgram
{
    /// <summary>
    /// This is an Utility class. Holds methods for accessing the filesytem.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// The method writes JArray content to a json file.
        /// </summary>
        /// <param name="obj">List of objects, which will be later serialized in to the json file.</param>
        /// <param name="fileName">FileName to where the writing will happen.</param>
        /// <returns>Task object.</returns>
        public static async Task WriteToFile(List<Data> obj, string fileName)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            List<Data> querry = await ReadFromFile(fileName);
            if (querry != null)
            {
                querry = querry.Concat(obj).ToList();
            }
            else
            {
                querry = obj;
            }
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            Debug.WriteLine($"[INFO] File writing happening right now at {fileName}");
            await FileIO.AppendTextAsync(sampleFile, JsonConvert.SerializeObject(querry, Formatting.Indented));
        }
        
        /// <summary>
        /// The method expects, that the contents of the file are in JArray format.
        /// The method opens the file if it already exits, and read text from it using async-await.
        /// </summary>
        /// <param name="fileName">FileName from where the reading will happen.</param>
        /// <returns>Contents of the file already serialized.</returns>
        public static async Task<List<Data>> ReadFromFile(string fileName)
        {
            Debug.WriteLine($"[INFO] File reading happening right now at {fileName}");
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            if (sampleFile == null)
            {
                sampleFile = await storageFolder.CreateFileAsync(fileName);
            }

            string jsonString = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            if (jsonString != null)
            {
                return (List<Data>)JsonConvert.DeserializeObject(jsonString, typeof(List<Data>));
            }
            return null;
        }
    }
}
