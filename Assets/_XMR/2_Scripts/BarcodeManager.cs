using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class BarcodeData
{
    public byte[] file;
    public string poid;
    public string plid;
}

[Serializable]
public class BarcodeResponse
{
    public BarcodeValue[] barcodeValue;
    public string imageURL;
    public string message;
}

[Serializable]
public class BarcodeValue
{
    public string barcode_data;
    public string barcode_type;
}

public class BarcodeManager : MonoBehaviour
{
    public static BarcodeManager Instance;

    string url;

    [SerializeField]
    string FilePath;
    [SerializeField]
    BarcodeResponse BarcodeResponse;

    [Space(10)]
    [SerializeField]
    GameObject BarcodePreviewUI;
    [SerializeField]
    RawImage previewImage;
    [SerializeField]
    TextMeshProUGUI sizeText;

    [Space(10)]
    [SerializeField]
    GameObject BarcodeResultUI;
    [SerializeField]
    RawImage inspectionImage;
    [SerializeField]
    RawImage poImage;
    [SerializeField]
    TMP_Text matchStatus;
    [SerializeField]
    Button barcodeSaveButton;

    byte[] fileData;


    private void Awake()
    {
        Instance = this;
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
        url = GlobalData.ApiLink + APIEndpoints.Instance.barcodeScannerEndPoint;

        Debug.Log(url);
    }

/*
    public void BarcodeScan()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        Debug.Log("Barcode scan screenshot");
        LivestreamUIManager.Instance.screenshotCategory = ScreenshotCategories.Barcode;
        ScreenshotManager.Instance.Screenshot();
    }
*/

    public void Preview(string path)
    {
        FilePath = path;

        fileData = File.ReadAllBytes(FilePath);
        Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(fileData);

        previewImage.texture = tex;

        if (PalmWheelManager.Instance.UseExternalCamera)
            previewImage.rectTransform.sizeDelta = new Vector2(240f, 180f);
        else
            previewImage.rectTransform.sizeDelta = new Vector2(154f, 180f);

        sizeText.text = "Image width:" + tex.width + " Image Height:" + tex.height;

        BarcodePreviewUI.SetActive(true);

        //previewImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
    }

    public void SendBarcodeForScan(Texture2D texture)
    {

        byte[] bodyRaw = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bodyRaw);
        form.AddField("plid", "66daf1c451c3ea7223a9b216");
        form.AddField("poid", "66daf17e51c3ea7223a9b0d4");

        //BarcodePreviewUI.SetActive(false);

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, url, form: form, callback: GetBarcodeResponse));
    }



    void GetBarcodeResponse(string response)
    {
        Debug.Log("barcode response : "+response);
        Debug.Log(response);
        BarcodeResponse = JsonUtility.FromJson<BarcodeResponse>(response);
        //BarcodeResultUI.SetActive(true);

        if(BarcodeResponse != null)
        {
            ScreenshotManager.Instance.SetText("barcode - message" + BarcodeResponse.message);
        }
        else
        {
            ScreenshotManager.Instance.SetText("barcode - could not parse response");
            Debug.Log("barcode - could not parse response");
        }
    }

    /*
    void PopulateUI()
    {
        Debug.Log("barcode - inspection image path : " + FilePath);

        byte[] fileData = File.ReadAllBytes(FilePath);
        Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(fileData);

        inspectionImage.texture = tex;
        //inspectionImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

        //StartCoroutine(PopulatePOImage());

        Debug.Log("barcode - message" + BarcodeResponse.message);
        matchStatus.text = BarcodeResponse.message;
    }

    IEnumerator PopulatePOImage()
    {
        Debug.Log("barcode - response po image url : " + BarcodeResponse.imageURL);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(BarcodeResponse.imageURL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
        {
            Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
            tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            poImage.texture = tex;
            //poImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }

        barcodeSaveButton.onClick.RemoveAllListeners();
        barcodeSaveButton.onClick.AddListener(delegate
        {
            BarcodeResultUI.SetActive(false);
            StartCoroutine(SaveCommentsManager.Instance.SaveComments(matchStatus.text));
        });
    }*/
}
