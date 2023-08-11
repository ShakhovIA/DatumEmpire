using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Scripts.Networking
{
    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }

    public static class WebMessage
    {
        public static async Task<T> Get<T>(string endpoint)
        {
            var getRequest = CreateRequest(endpoint);
            getRequest.SendWebRequest();

            while (!getRequest.isDone) await Task.Delay(10_000);//Timer.Wait(10));

            Debug.Log($"Request {getRequest.downloadHandler.text}");
            return JsonUtility.FromJson<T>(getRequest.downloadHandler.text);
        }

        public static async Task<string> Get(string endpoint)
        {
            var getRequest = CreateRequest(endpoint);
            getRequest.SendWebRequest();

            while (!getRequest.isDone) await Task.Delay(5_000);//Timer.Wait(5);

            Debug.Log($"Request {getRequest.downloadHandler.text}");
            return getRequest.downloadHandler.text;
        }

        public static async Task<T> Post<T>(string endpoint, object payload)
        {
            var postRequest = CreateRequest(endpoint, RequestType.POST, payload);
            postRequest.SendWebRequest();

            while (!postRequest.isDone) await Task.Delay(10_000);//Timer.Wait(10);
            Debug.Log($"Request {postRequest.downloadHandler.text}");
            return JsonUtility.FromJson<T>(postRequest.downloadHandler.text);
        }

        public static async Task<string> Post(string endpoint, object payload)
        {
            var postRequest = CreateRequest(endpoint, RequestType.POST, payload);
            postRequest.SendWebRequest();

            while (!postRequest.isDone) await Task.Delay(5_000);//Timer.Wait(5);
            Debug.Log($"Request {postRequest.downloadHandler.text}");
            return postRequest.downloadHandler.text;
        }

        private static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }
    }
}
