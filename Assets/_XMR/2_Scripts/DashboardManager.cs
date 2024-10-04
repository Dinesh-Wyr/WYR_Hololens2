using UnityEngine;

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

        LoginMetaUI.Instance.Loader(true);
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
        
        if(response.data == null)
        {
            return;
        }


        clearObjects();

        foreach(PLData dataItem in response.data)
        {
            GameObject obj = Instantiate(dashboardItem, listParent);
            obj.GetComponent<DashboardItem>().PopulateDashboardItemInfo(dataItem);
        }

        LoginMetaUI.Instance.Loader(false);

    }

}
