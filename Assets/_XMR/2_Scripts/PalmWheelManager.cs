using UnityEngine;

public enum PalmWheelTabs
{
    None,
    TwoPointGesture,
    TwoPointVoice,
    MultiPointVoice,
    MultiPointGesture,
    Defect
}

public class PalmWheelManager : MonoBehaviour
{
    private static PalmWheelManager _instance;

    public static PalmWheelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PalmWheelManager>();
            }

            return _instance;
        }
    }

    public PalmWheelTabs SelectedTab;

    public bool UseExternalCamera;
}
