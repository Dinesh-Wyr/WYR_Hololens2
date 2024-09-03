using UnityEngine;

public class TabBody : MonoBehaviour
{
    [SerializeField]
    TabGroups tabgroup;
    [SerializeField]
    GameObject content;

    void viewTabBody(TabGroups tabgroup)
    {
        if(tabgroup != this.tabgroup)
        {
            content.SetActive(false);
        }
        else
        {
            content.SetActive(true);
        }
    }

    private void OnEnable()
    {
        UIEventSystem.OnTabGroupChange += viewTabBody;
    }

    private void OnDisable()
    {
        UIEventSystem.OnTabGroupChange -= viewTabBody;
    }
}
