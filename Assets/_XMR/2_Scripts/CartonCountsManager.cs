using System.IO;
using UnityEngine;

public class CartonCountsManager : MonoBehaviour
{
    private static CartonCountsManager _instance;

    private string GetCartonCountURL;// = "https://ml.dev.wyrai.com/carton/carton_counter_fs";

    public static CartonCountsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CartonCountsManager>();
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        APIEndpoints.baseURLSet += SetURL;
    }

    private void OnDisable()
    {
        APIEndpoints.baseURLSet -= SetURL;
    }

    void SetURL()
    {
        GetCartonCountURL = APIEndpoints.Instance.GetCartonCountURL;

        Debug.Log(GetCartonCountURL);
        LoginMetaUI.Instance.Log(GetCartonCountURL);
    }
/*
    public void GetCartonCount(Texture2D texture)
    {

        //string filePath = ScreenshotManager.Instance.FilePath;
        //Debug.Log("Carton Count FilePath = " + filePath);
        //MRTKScreenshotManager.Instance.SetText("GetCartonCount");
        //byte[] bodyRaw = texture.GetRawTextureData();
        byte[] bodyRaw = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        Debug.Log("File Size: " + bodyRaw.Length);
        //string savePath = Application.dataPath + "/saved_image.png";
        //File.WriteAllBytes(savePath, bodyRaw);
       // MRTKScreenshotManager.Instance.SetText("File Size: " + bodyRaw.Length);
        StartCoroutine(ApiCallUtility.Instance.SendImage(Method.POST, GetCartonCountURL, bodyRaw, CartonCountCallback));
    }
*/

    public void GetCartonCount(Texture2D texture)
    {
        PreviewScreenshot.Instance.pngTextureBytes = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        PreviewScreenshot.Instance.pngImageName = "Cartons_" + System.DateTime.Now;
        LoginMetaUI.Instance.Loader(true);
        StartCoroutine(ApiCallUtility.Instance.SendImage(Method.POST, GetCartonCountURL, PreviewScreenshot.Instance.pngTextureBytes, CartonCountCallback));
    }

    public void CartonCountCallback(string responseText)
    {
        Debug.Log("Got Carton Count");
        LoginMetaUI.Instance.Log("Got Carton Count");
        CartonCountResponse response = JsonUtility.FromJson<CartonCountResponse>(responseText);
        string count = response.count.ToString();

        //StartCoroutine(SaveComments(count));
        PreviewScreenshot.Instance.DisplayCartonPreview(response.preview, count);
        LoginMetaUI.Instance.Loader(false);
    }
/*
    public void CartonCountCallback(string responseText)
    {
        MRTKScreenshotManager.Instance.SetText("CartonCountCallback");
        Debug.Log("Got Carton Count");
        CartonCountResponse response = JsonUtility.FromJson<CartonCountResponse>(responseText);
        string count = response.count.ToString();

        //StartCoroutine(SaveComments(count));
        MRTKScreenshotManager.Instance.DisplayCartonPreview(response.preview, count);

    }
*/


}
