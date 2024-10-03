using UnityEngine;
using UnityEngine.UI;

public class PalmWheelTab : MonoBehaviour
{
    [SerializeField]
    PalmWheelTabs tab;
    [SerializeField]
    Image imageComponent;
    [SerializeField]
    Color32 activatedColor;
    [SerializeField]
    Color32 deactivatedColor;
    [SerializeField]
    bool isActive;
    public void ToggleTab()
    {
        UIEventSystem.WheelTabChange(tab);
    }

    void OnWheelTabChange(PalmWheelTabs tab)
    {
       

        if (tab == this.tab)
        {
            
            isActive = !isActive;

            if(isActive)
            {
                PalmWheelManager.Instance.SelectedTab = tab;
                imageComponent.color = activatedColor;
            }
            else
            {
                PalmWheelManager.Instance.SelectedTab = PalmWheelTabs.None;
                imageComponent.color = deactivatedColor;
            }

            Debug.Log("Selected tab : "+PalmWheelManager.Instance.SelectedTab);
        }
        else
        {
            imageComponent.color = deactivatedColor;
            isActive = false;
        }
    }

    private void OnEnable()
    {
        UIEventSystem.OnWheelTabChange += OnWheelTabChange;
    }

    private void OnDisable()
    {
        UIEventSystem.OnWheelTabChange -= OnWheelTabChange;
    }
}
