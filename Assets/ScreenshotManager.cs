using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

#if WINDOWS_UWP
using Windows.Storage;
#endif


public class ScreenshotManager : MonoBehaviour
{

    public static ScreenshotManager Instance;

    [SerializeField] GameObject cameraContainer;
    [SerializeField] RawImage cameraLiveFeedImage;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI FPSText;

    bool isCameraOn;
    WebCamTexture webcamTexture;
    Texture2D screenshotTexture;


    int camWidth = 0;
    int camHeight = 0;
    int camRequestFPS = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;
        //InitializeCamera();

        //TakeScreenshot();
        //CountCartons();
    }

    private int fps;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = (int)(1.0f / deltaTime);

        // Display FPS in the console (optional)
        FPSText.text = fps.ToString();
    }

    public async void ShowCameraContainer()
    {
        isCameraOn = !isCameraOn;

        if (isCameraOn)
        {
            SetResolution(1920, 1080, 60);
            await InitializeCameraAsync();
        }
        else
        {
            cameraContainer.SetActive(false);

            webcamTexture.Stop();
        }
        
    }

    #region AR Screenshot

    PhotoCapture photoCaptureObject = null;
    public IEnumerator TakeARScreenshot()
    {
        Debug.Log("TakeARScreenshot");
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        if (isCartons)
            screenshotTexture = new Texture2D(cameraResolution.width/3, cameraResolution.height/3);
        else
            screenshotTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        yield return null;
        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(true, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                Debug.Log("StartPhotoModeAsync");
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        Debug.Log("OnCapturedPhotoToMemory");
        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(screenshotTexture);

        cameraLiveFeedImage.texture = screenshotTexture;
        cameraLiveFeedImage.rectTransform.sizeDelta = DesiredSize(screenshotTexture.width, screenshotTexture.height);
        cameraContainer.SetActive(true);
        if(isCartons)
        {
            isCartons = false;
            CartonCountsManager.Instance.GetCartonCount(screenshotTexture);
        }

        if(isBarcode)
        {
            isBarcode = false;
            BarcodeManager.Instance.SendBarcodeForScan(screenshotTexture);
        }
        // Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }


    public void DeleteARScreenshot()
    {
        cameraContainer.SetActive(false);
        Destroy(screenshotTexture);        
    }

    #endregion

    #region Screenshot
    public void TakeScreenshot()
    {
        //StartCoroutine(Screenshot());
        StartCoroutine(TakeARScreenshot());
    }
    #endregion

    #region Cartons

    bool isCartons;

    public void CountCartons()
    {
        SetText("CountCartons");
        isCartons = true;
        StartCoroutine(TakeARScreenshot());
    }

    public void DisplayCartonPreview(string base64String, string count)
    {
        SetText("DisplayCartonPreview");
        SetText("Total Cartons: " + count);
        statusText.gameObject.SetActive(true);

        byte[] data = Convert.FromBase64String(base64String);
        Texture2D tex = new(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(data); // Load data into the texture

        cameraLiveFeedImage.texture = tex;
    }


    #endregion

    #region Barcode

    bool isBarcode;

    public void ScanBarcode()
    {
        //isBarcode = true;
        //StartCoroutine(TakeARScreenshot());

        SetText("ScanBarcode");
        StartCoroutine(GetBarcodePic());
    }


    IEnumerator GetBarcodePic()
    {
        yield return StartCoroutine(ApplyTexture());
        SetText("GetBarcodePic");
        BarcodeManager.Instance.SendBarcodeForScan(screenshotTexture);
    }


    IEnumerator ApplyTexture()
    {
        SetText("Screenshot");
        screenshotTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
        Debug.Log(webcamTexture.isPlaying);
        // Copy the pixels from the WebCamTexture to the Texture2D
        if (webcamTexture.isPlaying)
        {
            screenshotTexture.SetPixels(webcamTexture.GetPixels());
            //statusText.text = "<color=white>GetPixels</color>";
        }
        yield return new WaitForEndOfFrame();
        screenshotTexture.Apply();

        cameraLiveFeedImage.texture = screenshotTexture;
        //statusText.gameObject.SetActive(false);
    }

    public void DeleteBarcode()
    {
        SetText("<color=red>Delete</color>");
        statusText.gameObject.SetActive(true);
        HideScreenshot();
    }

    void HideScreenshot()
    {
        if(!webcamTexture.isPlaying)
            webcamTexture.Play();

        cameraLiveFeedImage.texture = webcamTexture;
        Destroy(screenshotTexture);
    }

    #endregion

    #region Webcam

    //Initialize camera using coroutine
    private IEnumerator InitializeCamera()
    {
        webcamTexture = new WebCamTexture(camWidth, camHeight, camRequestFPS);
        cameraLiveFeedImage.texture = webcamTexture;
        webcamTexture.Play();

        yield return new WaitForEndOfFrame();
        // Set the RawImage's size based on the desired texture size
        cameraLiveFeedImage.rectTransform.sizeDelta = DesiredSize(webcamTexture.width, webcamTexture.height);

        // Assign the webcam texture to the RawImage
        cameraLiveFeedImage.texture = webcamTexture;


        // Start playing the webcam texture
        //webcamTexture.Play();
        cameraContainer.SetActive(true);
    }

    //Initialize camera using async task
    private async Task InitializeCameraAsync()
    {
        webcamTexture = new WebCamTexture(camWidth, camHeight, camRequestFPS);
        cameraLiveFeedImage.texture = webcamTexture;
        webcamTexture.Play();

        await Task.Yield();

        // Set the RawImage's size based on the desired texture size
        cameraLiveFeedImage.rectTransform.sizeDelta = DesiredSize(webcamTexture.width, webcamTexture.height);

        // Assign the webcam texture to the RawImage
        cameraLiveFeedImage.texture = webcamTexture;


        // Start playing the webcam texture
        //webcamTexture.Play();
        cameraContainer.SetActive(true);
    }

    public async void StartCamera(int index)
    {
        if (webcamTexture != null)
            webcamTexture.Stop();
        /*
                switch (index)
                {
                    case 0:
                        SetResolution(3840, 2160, 60);
                        break;
                    case 1:
                        SetResolution(3840, 2160, 50);
                        break;
                    case 2:
                        SetResolution(3840, 2160, 30);
                        break;
                    case 3:
                        SetResolution(1920, 1080, 60);
                        break;
                    case 4:
                        SetResolution(1920, 1080, 50);
                        break;
                    case 5:
                        SetResolution(1920, 1080, 30);
                        break;
                    case 6:
                        SetResolution(1280, 720, 60);
                        break;
                    case 7:
                        SetResolution(1280, 720, 50);
                        break;
                    case 8:
                        SetResolution(1280, 720, 30);
                        break;
                    case 9:
                        SetResolution(640, 360, 60);
                        break;
                    case 10:
                        SetResolution(640, 360, 50);
                        break;
                    case 11:
                        SetResolution(640, 360, 30);
                        break;
                }
        */
        SetResolution(1280, 720, 60);
        await InitializeCameraAsync();
    }

    #endregion

    #region Caculation
    public Vector2 DesiredSize(int width, int height)
    {
        // Get the webcam's native resolution
        int nativeWidth = width;
        int nativeHeight = height;

        // Calculate the maximum texture size based on the RawImage's size
        int maxWidth = 900;
        int maxHeight = 900;

        // Calculate the scale factor to fit the image within the RawImage while maintaining aspect ratio
        float scaleFactor = Mathf.Min((float)maxWidth / nativeWidth, (float)maxHeight / nativeHeight);

        // Apply the scale factor to get the desired texture size
        int desiredWidth = (int)(nativeWidth * scaleFactor);
        int desiredHeight = (int)(nativeHeight * scaleFactor);

        // Create the WebCamTexture with the desired size
        //webcamTexture = new WebCamTexture(device.name, desiredWidth, desiredHeight);

        // Set the RawImage's size based on the desired texture size
        //cameraLiveFeedImage.rectTransform.sizeDelta = 

        return new Vector2(desiredWidth, desiredHeight);
    }

    void SetResolution(int Width, int height, int fps)
    {
        camWidth = Width;
        camHeight = height;
        camRequestFPS = fps;
    }

    #endregion

    #region Save to Gallery
    public void SavePicture()
    {
        //SaveImageToGallery();
        //StartCoroutine(SaveToGallery());
    }

    public void SetText(string text)
    {
        statusText.text = text;
        statusText.gameObject.SetActive(true);
    }

    public async void SaveImageToGallery()
    {
        statusText.gameObject.SetActive(true);
#if WINDOWS_UWP
        if (screenshotTexture == null)
        {
            SetText("Image texture is not assigned.");
            return;
        }

        try
        {
            StorageFolder picturesLibrary = await DownloadsFolder.CreateFolderAsync("WYR");
            // Get the Pictures library
            //StorageFolder picturesLibrary = await StorageFolder.GetFolderFromPathAsync(KnownFolders.DocumentsLibrary.Path);

            // Generate a unique filename
            string fileName = "captured_image_.png";

            // Create a new file in the Pictures library
            StorageFile file = await picturesLibrary.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);

            // Encode the image texture to PNG
            byte[] bytes = screenshotTexture.EncodeToPNG();

            // Write the image data to the file
            await File.WriteAllBytesAsync(file.Path, bytes);

            SetText( "Image saved to gallery: " + file.Path);
        }
        catch (Exception ex)
        {
            SetText("Error saving image to gallery: " + ex.Message);
        }
#endif

    }

    IEnumerator SaveToGallery()
    {
        string folderPath = Application.persistentDataPath;

        SetText(folderPath);


        statusText.gameObject.SetActive(true);

        byte[] bytes = screenshotTexture.EncodeToPNG();
        File.WriteAllBytes(folderPath + "webcam_frame.png", bytes);

        yield return new WaitForEndOfFrame();

        SetText("<color=green>Saved</color>");
        statusText.gameObject.SetActive(true);
        HideScreenshot();
    }

    #endregion

}
