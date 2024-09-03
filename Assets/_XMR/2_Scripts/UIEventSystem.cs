using System;
public static class UIEventSystem
{
    public static event Action OnLogin;
    public static event Action OnLogout;

    public static event Action<Tabs> OnTabChange;
    public static event Action<TabGroups> OnTabGroupChange;

    public static event Action OnStartInspection;
    public static event Action OnEndInspection;

    public static event Action<PalmWheelTabs> OnWheelTabChange;

    public static event Action OnMeasureInCm;
    public static event Action OnMeasureInInch;


    public static void Login()
    {
        OnLogin?.Invoke();
    }

    public static void Logout()
    {
        OnLogout?.Invoke();
    }

    public static void TabChange(Tabs tab)
    {
        OnTabChange?.Invoke(tab);
    }

    public static void TabGroupChange(TabGroups tabGroup)
    {
        OnTabGroupChange?.Invoke(tabGroup);
    }

    public static void StartInspection()
    {
        OnStartInspection?.Invoke();
    }

    public static void EndInspection()
    {
        OnEndInspection?.Invoke();
    }

    public static void WheelTabChange(PalmWheelTabs tab)
    {
        OnWheelTabChange?.Invoke(tab);
    }

    public static void MeasureInCm()
    {
        OnMeasureInCm?.Invoke();
    }

    public static void MeasureInInch()
    {
        OnMeasureInInch?.Invoke();
    }
}
