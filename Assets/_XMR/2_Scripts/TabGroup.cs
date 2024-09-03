using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Tabs
{
    tab1,
    tab2,
    tab3,
    tab4,
    tab5,
    tab6,
    tab7,
    tab8,
    tab9,
}

public enum TabGroups
{
    Login,
    Livestream,
    Dashboard,      
    RequiredDocs,
    InspectionCriteria,
    tabGroup5,
    tabGroup6,
    tabGroup7,
    tabGroup8,
    tabGroup9,
}

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    TabGroups tabGroup;

    [Header("Tab change elements")]
    [SerializeField]
    Image tabBG;
    [SerializeField]
    Color32 tabBGUnselected;
    [SerializeField]
    Color32 tabBGSelected;
    [SerializeField]
    TMP_Text tabText;
    [SerializeField]
    Color32 tabTextUnselected;
    [SerializeField]
    Color32 tabTextSelected;
    [SerializeField]
    Image tick;
    [SerializeField]
    Color32 tickUnselected;
    [SerializeField]
    Color32 tickSelected;

    void tabSelected()
    {
        tabBG.color = tabBGSelected;
        tabText.color = tabTextSelected;
        tick.color = tickSelected;
    }

    void tabUnselected()
    {
        tabBG.color = tabBGUnselected;
        tabText.color = tabTextUnselected;
        tick.color = tickUnselected;
    }

    void onTabChange(TabGroups tabGroup)
    {
        if(tabGroup == this.tabGroup)
        {
            tabSelected();
        }
        else
        {
            tabUnselected();
        }
    }

    private void OnEnable()
    {
        UIEventSystem.OnTabGroupChange += onTabChange;
    }

    private void OnDisable()
    {
        UIEventSystem.OnTabGroupChange -= onTabChange;
    }
}
