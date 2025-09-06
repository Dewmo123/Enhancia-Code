using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Network
{
    public class WebClient
    {
        public string ServerUrl { get; private set; }
        public WebClient(string serverUrl)
        {
            ServerUrl = serverUrl;
        }
        public async void SendPostRequest<T>(string endPoint, T information, Action<bool> callback) where T : class
        {
            using UnityWebRequest request = new UnityWebRequest($"{ServerUrl}/{endPoint}", "POST");
            string json = JsonConvert.SerializeObject(information);
            byte[] data = Encoding.UTF8.GetBytes(json);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(data);

            Debug.Log("Send Post");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            await request.SendWebRequest();
            Debug.Log(request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                bool tf = false;
                bool.TryParse(request.downloadHandler.text, out tf);
                Debug.Log(tf);
                callback(tf);
            }
            else
                callback(false);
        }
        public async void SendGetRequest<T>(string endpoint, Action<T> callback)
        {
            using var request = UnityWebRequest.Get($"{ServerUrl}/{endpoint}");
            Debug.Log($"Send GetRequest");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var jsonResult = Encoding.UTF8.GetString(request.downloadHandler.data);
                callback(JsonConvert.DeserializeObject<T>(jsonResult));
            }
            else
            {
                Debug.LogWarning($"Failed SendGetRequest: {request.result}");
                callback(default(T));
            }
        }
        public async void SendDeleteRequest(string endPoint, Action<bool> callback)
        {
            using var request = UnityWebRequest.Delete($"{ServerUrl}/{endPoint}");
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                bool success = false;
                bool.TryParse(request.downloadHandler.text, out success);
                callback(success);
            }
            else
            {
                callback(false);
            }
        }
        public async void SendPatchRequest<T>(string endPoint, T information, Action<bool> callback)
        {
            using UnityWebRequest request = new UnityWebRequest($"{ServerUrl}/{endPoint}", "PATCH");
            string json = JsonConvert.SerializeObject(information, Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(json);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(data);

            Debug.Log("Send Patch");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            await request.SendWebRequest();
            Debug.Log(request.result);
            if (request.result == UnityWebRequest.Result.Success)
            {
                bool tf = false;
                bool.TryParse(request.downloadHandler.text, out tf);
                Debug.Log(tf);
                callback(tf);
            }
            else
                callback(false);

        }
    }
}