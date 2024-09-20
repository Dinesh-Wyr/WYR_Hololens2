using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum Method
{
    GET,
    POST,
    DELETE,
    PUT
}

public class ApiCallUtility : MonoBehaviour
{

    public static ApiCallUtility Instance;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public IEnumerator APIRequest(Method method, string uri, string json = null, WWWForm form = null, Action<string> callback = null)
    {
        UnityWebRequest www = new UnityWebRequest();

        switch (method)
        {
            case Method.GET:
                www = UnityWebRequest.Get(uri);
                break;
            case Method.POST:
                if (form != null)
                    www = UnityWebRequest.Post(uri, form);
                else
                {
                    WWWForm form1 = new WWWForm();
                    www = UnityWebRequest.Post(uri, form1);
                }
                break;
            case Method.DELETE:
                www = UnityWebRequest.Delete(uri);
                break;
            case Method.PUT:
                www = new UnityWebRequest(uri, Method.PUT.ToString());
                break;
        }

        
        if (!string.IsNullOrEmpty(json))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
        }

        www.SetRequestHeader("authorization", "Bearer " + GlobalData.Token);

        yield return www.SendWebRequest();
        Debug.Log(www.result);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            Debug.LogError(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (callback != null && !string.IsNullOrEmpty(www.downloadHandler.text))
            {
                Debug.Log("post callback");
                callback?.Invoke(www.downloadHandler.text);
            }
        }

        www.Dispose();
    }



    public IEnumerator SendImage(Method method, string url, byte[] data, Action<string> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method.ToString()))
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "image/png");

            Debug.Log(method.ToString());

            //if (method == Method.POST)
                //request.SetRequestHeader("Authorization", "Bearer " + GlobalData.Token);

            yield return request.SendWebRequest();

            Debug.Log("done");

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                MRTKScreenshotManager.Instance.SetText("Error Posting image: " + request.error);
                Debug.Log("Error Posting image: " + request.error);
                yield break;
            }
            else
            {
                MRTKScreenshotManager.Instance.SetText("FILE UPLOADED");
                Debug.Log("FILE UPLOADED");
                Debug.Log("Server response: " + request.downloadHandler.text);
                if (callback != null)
                {
                    callback(request.downloadHandler.text);
                }
            }
        }
    }

    public IEnumerator DelayedCall(float  delay, Action function) 
    {
        yield return new WaitForSeconds(delay);

        function();
    }
}
