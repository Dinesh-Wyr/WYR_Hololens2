using UnityEngine;
using UnityEngine.UI;

public class PalmWheelCameraControl : MonoBehaviour
{
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
    public void ToggleTab()
    {

        if (MRTKScreenshotManager.Instance.isScreenshot && MRTKScreenshotManager.Instance.isCartons)
            return;

        isActive = !isActive;

        if (isActive)
        {
            imageComponent.color = activatedColor;
            //if (QuestGuideFrame != null)
            //    QuestGuideFrame.SetActive(false);
        }
        else
        {
            imageComponent.color = deactivatedColor;
            //if (QuestGuideFrame != null)
              //  QuestGuideFrame.SetActive(true);
        }

        //PalmWheelManager.Instance.UseExternalCamera = isActive;
    }
}
