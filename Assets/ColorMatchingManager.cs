using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ImageUploadResponse
{
    public string message;
    public string url;
}

[Serializable]
public class ColorMatchRequest
{
    public string poid;
    public string plid;
    public string approved;
    public string candidate;
}

public class ColorMatchResponse
{
    public string message;
}

public class ColorMatchingManager : MonoBehaviour
{
    public static ColorMatchingManager Instance;

    [SerializeField] GameObject colorMatchResultObject;
    [SerializeField] RawImage approvedImage;
    [SerializeField] RawImage inpectionImage;
    [SerializeField] TextMeshProUGUI matchStatus;

    string uploadImageURL;
    string colorMatchURL;

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
        uploadImageURL = GlobalData.ApiLink + APIEndpoints.Instance.uploadImageEndPoint;
        colorMatchURL = GlobalData.ApiLink + APIEndpoints.Instance.colorMatchEndPoint;

    }


    static Texture2D approvedTempTexture;
    static string approvedImageURL = string.Empty;

    public void SendApprovedToServer(Texture2D texture)
    {
        approvedTempTexture = new Texture2D(texture.width, texture.height);
        approvedTempTexture.SetPixels(texture.GetPixels());
        approvedTempTexture.Apply();

        WWWForm form = ReturnForm("Approved", approvedTempTexture);

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, uploadImageURL, form: form, callback: ApprovedSampleResponse));
    }

    void ApprovedSampleResponse(string response)
    {
        ImageUploadResponse imageUploadResponse = JsonUtility.FromJson<ImageUploadResponse>(response);

        approvedImageURL = imageUploadResponse.url;


        MRTKScreenshotManager.Instance.DeletePreview();
        MRTKScreenshotManager.Instance.SwitchApprovedUpload(true);
        MRTKScreenshotManager.Instance.CheckApprovedUploaded();
        MRTKScreenshotManager.Instance.Loader(false);
    }

    static Texture2D inspectionTempTexture;
    static string inspectionImageURL = string.Empty; 

    public void SendInspectionToServer(Texture2D texture)
    {
        inspectionTempTexture = new Texture2D(texture.width, texture.height);
        inspectionTempTexture.SetPixels(texture.GetPixels());
        inspectionTempTexture.Apply();

        WWWForm form = ReturnForm("Inspection", inspectionTempTexture);

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, uploadImageURL, form: form, callback: InspectionSampleResponse));
    }

    void InspectionSampleResponse(string response)
    {
        ImageUploadResponse imageUploadResponse = JsonUtility.FromJson<ImageUploadResponse>(response);

        inspectionImageURL = imageUploadResponse.url;

        ColorMatchRequest colorMatchRequest = new ColorMatchRequest();
        colorMatchRequest.poid = GlobalData.poid;
        colorMatchRequest.plid = GlobalData.plid;
        colorMatchRequest.approved = approvedImageURL;
        colorMatchRequest.candidate = inspectionImageURL;

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, colorMatchURL, JsonUtility.ToJson(colorMatchRequest), callback: MatchResponse));
    }

    void MatchResponse(string response)
    {
        ColorMatchResponse colorMatchResponse = JsonUtility.FromJson<ColorMatchResponse>(response);

        approvedImage.texture = approvedTempTexture;
        inpectionImage.texture = inspectionTempTexture;

        matchStatus.text = colorMatchResponse.message;

        
        colorMatchResultObject.SetActive(true);

        MRTKScreenshotManager.Instance.DeletePreview();
        MRTKScreenshotManager.Instance.SwitchApprovedUpload(false);
        MRTKScreenshotManager.Instance.CheckApprovedUploaded();
        MRTKScreenshotManager.Instance.Loader(false);

    }

    WWWForm ReturnForm(string imageName, Texture2D texture)
    {
        PreviewScreenshot.Instance.pngTextureBytes = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        PreviewScreenshot.Instance.pngImageName = imageName + "_" + System.DateTime.Now;
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", PreviewScreenshot.Instance.pngTextureBytes);
        form.AddField("plid", GlobalData.plid);
        form.AddField("poid", GlobalData.poid);
        return form;
    }


}
