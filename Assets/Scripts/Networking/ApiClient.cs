using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public static class ApiClient
{
    // IMPORTANT: replace with your backend URL or use environment config
    public static string BaseUrl = "https://your-backend.example.com"; // placeholder

    public static IEnumerator Post<T>(string path, string jsonBody, Action<T> onSuccess, Action<string> onError = null)
    {
        var url = BaseUrl + path;
        using (UnityWebRequest uwr = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
#else
            if (uwr.isNetworkError || uwr.isHttpError)
#endif
            {
                onError?.Invoke(uwr.error);
            }
            else
            {
                try
                {
                    var text = uwr.downloadHandler.text;
                    var obj = JsonUtility.FromJson<T>(text);
                    onSuccess?.Invoke(obj);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex.Message);
                }
            }
        }
    }

    public static IEnumerator Get<T>(string path, Action<T> onSuccess, Action<string> onError = null)
    {
        var url = BaseUrl + path;
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
#else
            if (uwr.isNetworkError || uwr.isHttpError)
#endif
            {
                onError?.Invoke(uwr.error);
            }
            else
            {
                try
                {
                    var text = uwr.downloadHandler.text;
                    var obj = JsonUtility.FromJson<T>(text);
                    onSuccess?.Invoke(obj);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex.Message);
                }
            }
        }
    }
}
