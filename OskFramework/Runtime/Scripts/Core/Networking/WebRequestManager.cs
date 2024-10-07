using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
public class WebRequestManager : GameFrameworkComponent
{
    // GET request
    public void Get(string url, System.Action<string> onSuccess, System.Action<string> onError)
    {
        StartCoroutine(HandleRequest(UnityWebRequest.Get(url), onSuccess, onError));
    }

    // POST request with JSON data
    public void Post(string url, string jsonData, System.Action<string> onSuccess, System.Action<string> onError)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(HandleRequest(webRequest, onSuccess, onError));
    }

    // PUT request with JSON data
    public void Put(string url, string jsonData, System.Action<string> onSuccess, System.Action<string> onError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonData);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(HandleRequest(webRequest, onSuccess, onError));
    }

    // DELETE request
    public void Delete(string url, System.Action<string> onSuccess, System.Action<string> onError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete(url);

        StartCoroutine(HandleRequest(webRequest, onSuccess, onError));
    }

    // Coroutine to handle the request and return results
    private IEnumerator HandleRequest(UnityWebRequest webRequest, System.Action<string> onSuccess, System.Action<string> onError)
    {
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Request failed: " + webRequest.error);
            onError?.Invoke(webRequest.error);
        }
        else
        {
            Debug.Log("Request successful: " + webRequest.downloadHandler.text);
            onSuccess?.Invoke(webRequest.downloadHandler.text);
        }
    }
}

}