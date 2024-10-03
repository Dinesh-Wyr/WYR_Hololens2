using UnityEngine;
using UnityEngine.UI;


public enum CameraTabs
{
    Barcode,
    ColorMatch
}

public class PalmWheelCameraControl : MonoBehaviour
{
    [SerializeField] CameraTabs cameraTab;

    [SerializeField]
    Image imageComponent;
    [SerializeField]
    Color32 activatedColor;
    [SerializeField]
    Color32 deactivatedColor;
    [SerializeField]
    bool isActive;

    [Space(10)]
    [SerializeField]
    GameObject QuestGuideFrame;

    private void OnEnable()
    {
        UIEventSystem.OnCameraTabChange += OnCameraTabChange;
    }

    private void OnDisable()
    {
        UIEventSystem.OnCameraTabChange -= OnCameraTabChange;
    }

    public void ToggleTab()
    {
        

        if (MRTKScreenshotManager.Instance.isScreenshot && MRTKScreenshotManager.Instance.isCartons)
            return;

        UIEventSystem.CameraTabChange(cameraTab);
    }

    void OnCameraTabChange(CameraTabs tab)
    {
        if (MRTKScreenshotManager.Instance.isCameraOn && MRTKScreenshotManager.Instance.selectedCameraTab != tab)
            return;


        if (tab == this.cameraTab)
        {
            isActive = !isActive;

            if (isActive)
            {
                imageComponent.color = activatedColor;

            }
            else
            {
                imageComponent.color = deactivatedColor;
            }

            MRTKScreenshotManager.Instance.SwitchCameraContainer(tab);
        }
        else
        {
            imageComponent.color = deactivatedColor;
            isActive = false;
        }
    }
}
