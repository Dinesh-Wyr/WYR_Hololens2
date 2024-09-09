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
    }

    public void GetCartonCount(Texture2D texture)
    {

        //string filePath = ScreenshotManager.Instance.FilePath;
        //Debug.Log("Carton Count FilePath = " + filePath);
        ScreenshotManager.Instance.SetText("GetCartonCount");
        //byte[] bodyRaw = texture.GetRawTextureData();
        byte[] bodyRaw = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        Debug.Log("File Size: " + bodyRaw.Length);
        //string savePath = Application.dataPath + "/saved_image.png";
        //File.WriteAllBytes(savePath, bodyRaw);
        ScreenshotManager.Instance.SetText("File Size: " + bodyRaw.Length);
        StartCoroutine(ApiCallUtility.Instance.SendImage(Method.POST, GetCartonCountURL, bodyRaw, CartonCountCallback));
    }


    public void CartonCountCallback(string responseText)
    {
        ScreenshotManager.Instance.SetText("CartonCountCallback");
        Debug.Log("Got Carton Count");
        CartonCountResponse response = JsonUtility.FromJson<CartonCountResponse>(responseText);
        string count = response.count.ToString();

        //StartCoroutine(SaveComments(count));
        ScreenshotManager.Instance.DisplayCartonPreview(response.preview, count);

    }

}
