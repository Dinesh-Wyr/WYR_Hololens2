using UnityEngine;

public class DashboardResponse
{
    public string message;
    public PLData[] PlDataFind;
}

public class DashboardManager : MonoBehaviour
{
    [SerializeField]
    GameObject dashboardItem;
    [SerializeField]
    Transform listParent;

    string uri;


    private void OnEnable()
    {
        SetURL();
    }

    private void SetURL()
    {
        uri = GlobalData.ApiLink + APIEndpoints.Instance.dashboardEndPoint;

        Debug.Log(uri);

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.GET, uri,callback: LoadData));
    }


    void clearObjects()
    {
        for (int i = 1; i < listParent.childCount; i++)
        {
            Destroy(listParent.GetChild(i).gameObject);
        }
    }

    void LoadData(string json)
    {
        DashboardResponse response = JsonUtility.FromJson<DashboardResponse>(json);
        
        if(response.PlDataFind == null)
        {
            return;
        }

        clearObjects();

        foreach(PLData dataItem in response.PlDataFind)
        {
            GameObject obj = Instantiate(dashboardItem, listParent);
            obj.GetComponent<DashboardItem>().PopulateDashboardItemInfo(dataItem);
        }
        
    }

}
