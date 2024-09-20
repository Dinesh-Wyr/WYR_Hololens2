using MixedReality.Toolkit.UX;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PreviewScreenshot : MonoBehaviour
{

    [SerializeField] private GameObject ScreenshotPreviewObj;
    [SerializeField] private GameObject ScreenshotPreview;
    [SerializeField] private GameObject CommentsConfirmation;
    [SerializeField] private GameObject DefectsSection;
    [SerializeField] private GameObject CartonSection;
    [SerializeField] private TMPro.TMP_Text CartonCount;
    [SerializeField] private PressableButton CartonSaveButton;
    public static PreviewScreenshot Instance;
    public static string defectCategory;
    [SerializeField] private Toggle NoneCategory;
    private static bool setToNone = false;

    

    public void Awake()
    {
        if (Instance == null)
            Instance = this;


        defectCategory = "None";
    }

    private void Update()
    {
        if (ScreenshotPreviewObj.activeSelf && setToNone == false)
        {
            ResetToNone();
            setToNone = true;
        }
    }


    /// <summary>
    /// Loading cropped image texture onto the preview prefab.
    /// </summary>
    /// <param name="imagePath">path of texture to be loaded</param>
    private void LoadImage(string imagePath)
    {
        Debug.Log("LoadImage");

        byte[] fileData = File.ReadAllBytes(imagePath);
        Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(fileData); // Load data into the texture

        RawImage imageToDisplay = ScreenshotPreview.GetComponent<RawImage>();

        if (PalmWheelManager.Instance.UseExternalCamera)
            imageToDisplay.rectTransform.sizeDelta = new Vector2(240f, 180f);
        else
            imageToDisplay.rectTransform.sizeDelta = new Vector2(154f, 180f);

        if (imageToDisplay != null)
        {
            imageToDisplay.texture = tex;
        }
        else
        {
            Debug.Log("NULL REFERENCE of IMAGE COMPONENT");
        }
    }

    /// <summary>
    /// Displaying the screenshot prefab 
    /// </summary>
    public void DisplayPreview(string path)
    {
        Debug.Log("DisplayPreview");

        try
        {
            setToNone = false;
            ScreenshotPreviewObj.SetActive(true);
            DefectsSection.SetActive(true);
            CartonSection.SetActive(false);
            LoadImage(path);
        }
        catch (System.Exception e) {
            Debug.Log(" error in Showing screenshot " + e);
        }
       
    }

    [HideInInspector]
    public byte[] pngTextureBytes;

    [HideInInspector]
    public string pngImageName;
    public void DisplayPreview(Texture2D texture)
    {
        pngTextureBytes = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat, (uint)texture.width, (uint)texture.height);
        //Debug.Log(System.DateTime.Now);
        pngImageName = "Screenshot_" + System.DateTime.Now;
        setToNone = false;
        ScreenshotPreviewObj.SetActive(true);
        DefectsSection.SetActive(true);
        CartonSection.SetActive(false);
        ScreenshotPreview.GetComponent<RawImage>().texture = texture;
    }

    public void DisplayCartonPreview(string base64String, string count)
    {

        try
        {
            Debug.Log("Carton preview");
            setToNone = false;
            ScreenshotPreviewObj.SetActive(true);
            DefectsSection.SetActive(false);
            CartonSection.SetActive(true);

            CartonCount.text = "Count : " + count;

            byte[] data = System.Convert.FromBase64String(base64String);
            Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(data); // Load data into the texture

            RawImage imageToDisplay = ScreenshotPreview.GetComponent<RawImage>();

            //if (PalmWheelManager.Instance.UseExternalCamera)
                imageToDisplay.rectTransform.sizeDelta = new Vector2(240f, 180f);
            //else
                //imageToDisplay.rectTransform.sizeDelta = new Vector2(154f, 180f);

            if (imageToDisplay != null)
            {
                imageToDisplay.texture = tex;
            }
            else
            {
                Debug.Log("NULL REFERENCE of IMAGE COMPONENT");
            }

            CartonSaveButton.OnClicked.RemoveAllListeners();
            CartonSaveButton.OnClicked.AddListener(delegate {
                ScreenshotPreviewObj.SetActive(false);
                StartCoroutine(SaveCommentsManager.Instance.SaveComments(count)); 
            });
        }
        catch (System.Exception e)
        {
            Debug.Log("error in carton screenshot preview : " + e.Message);
        }
    }


    /// <summary>
    /// when user presses the delete button prefab is hidden. 
    /// or when user saves the comments for the screenshot.
    /// </summary>
    public void DeleteScreenshot()
    {
        //check if prefab is visible
        if(!ScreenshotPreviewObj.activeSelf)
        {
            return;
        }
        try
        {
            ScreenshotPreview.GetComponent<RawImage>().texture = null;
            ScreenshotPreviewObj.SetActive(false);
            Debug.Log("Deleting Screenshot");
        }
        catch (System.Exception e) 
        {
            Debug.Log("error deleting screenshot " + e);
        }
        
    }


    /// <summary>
    /// when user chooses to save the screenshot a keyboard is opened to take comments.
    /// </summary>
    public void AddComments()
    {
        try
        {
            CommentsConfirmation.SetActive(false);
            KeyboardManager.Instance.OpenCommentsKeyboard();
            Debug.Log(" Opening Keyboard...");
        }
        catch (System.Exception e)
        {
            Debug.Log("error saving screenshot" + e);
        }
    }

    /// <summary>
    /// take confirmation for adding comments
    /// </summary>
    public void TakeConfirmation()
    {
        try
        {            
            Debug.Log("Displaying Confirmation....");
            ScreenshotPreview.GetComponent<RawImage>().texture = null;
            ScreenshotPreviewObj.SetActive(false);
            CommentsConfirmation.SetActive(true);
            //QuitApplication.instance.PositionObjectFrontOfCamera(CommentsConfirmation, 0.2f);
            Debug.Log("Showing comments confirmation");
           

        }
        catch (System.Exception e)
        {
            Debug.Log(" error in Showing comments confirmation " + e);
        }
    }

    /// <summary>
    /// user does not want to add comments
    /// </summary>
    public void DontAddComments()
    {
        try
        {
            CommentsConfirmation.SetActive(false);
            StartCoroutine(SaveCommentsManager.Instance.SaveComments(""));
            Debug.Log("Uploaded Image without comments...");
        }
        catch (System.Exception e) 
        {
            Debug.Log(" error uploading image without comments..." + e);
        }
       
    }

    /// <summary>
    /// user selects defect category to None
    /// </summary>
    /// <param name="valueChanged">toggle parameter , function exectuted when true</param>
    public void SetDefectCategoryNone(bool valueChanged)
    {
        defectCategory = "None";
    }

    /// <summary>
    /// user selects defect category to Minor
    /// </summary>
    /// <param name="valueChanged">toggle parameter , function exectuted when true</param>
    public void SetDefectCategoryMinor(bool valueChanged)
    {
        defectCategory = "Minor";
       
    }

    /// <summary>
    /// user selects defect category to Major
    /// </summary>
    /// <param name="valueChanged">toggle parameter , function exectuted when true</param>
    public void SetDefectCategoryMajor(bool valueChanged)
    {
        defectCategory = "Major";
    }

    /// <summary>
    /// user selects defect category to Critical
    /// </summary>
    /// <param name="valueChanged">toggle parameter , function exectuted when true</param>
    public void SetDefectCategoryCritical(bool valueChanged)
    {
        defectCategory = "Critical";
    }

    /// <summary>
    /// set the default defect type to None
    /// </summary>
    public void ResetToNone()
    {
        NoneCategory.isOn = true;
        defectCategory = "None";
        Debug.Log("Resetting to None");
    }

}
