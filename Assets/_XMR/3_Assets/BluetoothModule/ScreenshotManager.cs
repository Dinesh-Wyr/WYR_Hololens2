using System;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    private static ScreenshotManager _instance;

    public static ScreenshotManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScreenshotManager>();
            }

            return _instance;
        }
    }

    public string FilePath = "";
    public string FileName = "";


    public void GetFilepathAndProcess(string path, ScreenshotCategories category = ScreenshotCategories.Normal)
    {/*
        try
        {
            Debug.Log(path.Split('/')[1]);
            FilePath = path;
            FileName = path.Split('/')[1];
            //Debug.Log("Getting Image file path at " + System.DateTime.Now + " time taken = " + (System.DateTime.Now - SendMessageOverBridge.start).TotalMilliseconds);

            switch (category)
            {
                case ScreenshotCategories.Normal:
                    PreviewScreenshot.Instance.DisplayPreview(FilePath);
                    break;
                case ScreenshotCategories.Carton:
                    CartonCountsManager.Instance.GetCartonCount();
                    break;
                case ScreenshotCategories.Barcode:
                    BarcodeManager.Instance.Preview(FilePath);
                    break;
            }
        }
        catch (Exception e)
        {
           Debug.Log("Error getting file path " + e);
        }
        */
    }

    #region Screenshot

    public DateTime start;
    public CameraModes cameraMode;

    /// <summary>
    /// executes the screenshot command
    /// </summary>    
    public void Picture()
    {
        try
        {
            KeyboardManager.Instance.closeKeyboard();
            // timer = Time.time;
            start = DateTime.Now;
            //Debug.Log("Command received for screenshot at " + System.DateTime.Now + " time taken = " + (System.DateTime.Now - start).TotalMilliseconds);
            Debug.Log("Picture");
            PreviewScreenshot.Instance.DeleteScreenshot();

            Screenshot();
            LivestreamUIManager.Instance.screenshotCategory = ScreenshotCategories.Normal;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error taking picture: " + ex.Message);
        }
    }


    public void Screenshot()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        Debug.Log("Screenshot");

        if (LivestreamUIManager.Instance.IsConnected)
        {
            if (PalmWheelManager.Instance.UseExternalCamera)
            {
                Debug.Log("external camera ss called");
                //BluetoothManager.BluetoothConnector.CallStatic("WriteData", "ecs");
                LivestreamUIManager.Instance.TakeScreenShot("ecs");
                cameraMode = CameraModes.External;
            }
            else
            {
                Debug.Log("quest camera ss called");
                //BluetoothManager.BluetoothConnector.CallStatic("WriteData", "sc");
                LivestreamUIManager.Instance.TakeScreenShot("sc");
                cameraMode = CameraModes.Internal;
            }

        }
    }

    #endregion
}
