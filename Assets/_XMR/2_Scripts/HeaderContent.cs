using UnityEngine;
using TMPro;

public class HeaderContent : MonoBehaviour
{
    [SerializeField]
    GameObject LogoutIcon;
    [SerializeField]
    GameObject CloseIcon;
    [SerializeField]
    GameObject MicIcon;
    [SerializeField]
    GameObject PauseButton;
    [SerializeField]
    GameObject EndButton;
    [SerializeField]
    GameObject PoPlSection;
    [SerializeField]
    TMP_Text PoNumber;
    [SerializeField]
    TMP_Text PlNumber;

    public void EndInspection()
    {
        LoginMetaUI.Instance.ShowEndInspectionConfirmation();
    }

    void viewLoggedinIcons()
    {
        LogoutIcon.SetActive(true);
        MicIcon.SetActive(true);
    }

    void hideLoggedinIcons()
    {
        LogoutIcon.SetActive(false);
        MicIcon.SetActive(false);
    }

    void viewInspectionButtons()
    {
        PauseButton.SetActive(true);
        EndButton.SetActive(true);
    }

    void hideInspectionButtons()
    {
        PauseButton?.SetActive(false);
        EndButton?.SetActive(false);
    }

    void viewPoPlSection()
    {
        PoPlSection.SetActive(true);
        PoNumber.text = "PO : "+GlobalData.poid;
        PlNumber.text = "PL : "+GlobalData.plid;
    }

    void hidePoPlSection()
    {
        PoPlSection?.SetActive(false);
    }

    private void OnEnable()
    {
        UIEventSystem.OnLogin += viewLoggedinIcons;
        UIEventSystem.OnLogout += hideLoggedinIcons;
        UIEventSystem.OnStartInspection += viewInspectionButtons;
        UIEventSystem.OnStartInspection += viewPoPlSection;
        UIEventSystem.OnEndInspection += hideInspectionButtons;
        UIEventSystem.OnEndInspection += hidePoPlSection;
    }

    private void OnDisable()
    {
        UIEventSystem.OnLogout -= hideLoggedinIcons;
        UIEventSystem.OnLogin -= viewLoggedinIcons;
        UIEventSystem.OnStartInspection -= viewInspectionButtons;
        UIEventSystem.OnStartInspection -= viewPoPlSection;
        UIEventSystem.OnEndInspection -= hideInspectionButtons;
        UIEventSystem.OnEndInspection -= hidePoPlSection;
    }
}
