using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveCommentsManager : MonoBehaviour
{
    private static SaveCommentsManager _instance;

    [SerializeField] private TMP_InputField userComments;

    private string S3UploadURL = ""; // This will be obtained from the presigner URL response
    private string GetS3PresignUrl;
    private string S3PostCommentsURL;

    public static SaveCommentsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveCommentsManager>();
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
        GetS3PresignUrl = GlobalData.ApiLink + APIEndpoints.Instance.GetS3EndPoint;
        S3PostCommentsURL = GlobalData.ApiLink + APIEndpoints.Instance.S3PostCommentsEndPoint;

        Debug.Log(GetS3PresignUrl);
        Debug.Log(S3PostCommentsURL);
    }

    #region SaveComments

    /// <summary>
    /// saves user comments
    /// </summary>

    public void SaveComments()
    {
        try
        {
            if (!KeyboardManager.isKeyboardOn)
                return;

            Debug.Log("Saving comments.");
            PreviewScreenshot.Instance.DeleteScreenshot();
            Debug.Log(userComments.text);
            StartCoroutine(SaveComments(userComments.text));

            KeyboardManager.Instance.closeKeyboard();
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error saving comments: " + ex.Message);
        }
    }


    /// <summary>
    /// saves user comments
    /// </summary>
    /// <param name="Usercomments"> user comments</param>
    public IEnumerator SaveComments(string Usercomments)
    {
        Debug.Log("Input field string " + Usercomments);

        // parse user comments and search for "/n" which means next comment.
        string[] splitStr = Usercomments.Split('\n');
        List<string> commentsList = new(splitStr);

        Debug.Log("COMMENTS = " + commentsList);


        string jsonData = JsonUtility.ToJson(new S3RequestData(GlobalData.poid, GlobalData.plid, "image/png", PreviewScreenshot.Instance.pngImageName));

        Debug.Log("presigned json : " + jsonData);

        yield return StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, GetS3PresignUrl, jsonData,callback: UploadImage));

        // send image url and comments to server
        if (S3UploadURL == null)
        {
            Debug.Log("Upload Failed");
            yield break;
        }

        yield return StartCoroutine(SendImageURLToServer(S3UploadURL, GlobalData.poid, GlobalData.plid, commentsList));
        userComments.text = string.Empty;
        Debug.Log("Comments Saved!!!");
    }

    #endregion

    #region ImageUpload

    public void UploadImage(string responseText)
    {
        GetS3URLResponse response = JsonUtility.FromJson<GetS3URLResponse>(responseText);
        S3UploadURL = response.url;
        Debug.Log("GOT PRESIGN URL = " + S3UploadURL);

        // Get IMAGE FROM LOCATION
        //string filePath = ScreenshotManager.Instance.FilePath;
        //Debug.Log("FilePath = " + filePath);
        //byte[] jsonBytes = File.ReadAllBytes(filePath);
        //Debug.Log("File Read");

        // UPLOAD TO S3
        StartCoroutine(ApiCallUtility.Instance.SendImage(Method.PUT, S3UploadURL, PreviewScreenshot.Instance.pngTextureBytes));
    }


    // post request to send data.
    private IEnumerator SendImageURLToServer(string imageURL, string poid, string plid, List<string> commentsList)
    {
        if (imageURL == null)
        {
            Debug.Log("Take a ScreenShot First");
        }

        
        string jsonData = JsonUtility.ToJson(new SendCommentsWithDefectType(imageURL, poid, plid, commentsList, PreviewScreenshot.defectCategory));

        Debug.Log("Sending image URL to server: " + imageURL);
        Debug.Log("Payload: " + jsonData);

        // Send image URL to server

        yield return StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, S3PostCommentsURL, jsonData));

    }

    #endregion

}
