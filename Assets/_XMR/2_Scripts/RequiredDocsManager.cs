using System;
using System.Collections.Generic;
using UnityEngine;


public class RequiredDocsManager : MonoBehaviour
{
    [SerializeField]
    GameObject requiredDocsItem;
    [SerializeField]
    Transform listParent;

    [Space]
    [SerializeField]
    bool isVisited;
    [SerializeField]
    bool isSkipped;

    [Space]
    [SerializeField]
    PageMode mode;

    string getUrl;
    string updateUrl;

    private void OnEnable()
    {
        SetURL();
    }

    private void SetURL()
    {
        getUrl = GlobalData.ApiLink + APIEndpoints.Instance.getDocsEndPoint + GlobalData.productID;
        updateUrl = GlobalData.ApiLink + APIEndpoints.Instance.updateDocsEndPoint;

        Debug.Log(getUrl);
        Debug.Log(updateUrl);

        StartCoroutine(ApiCallUtility.Instance.DelayedCall(1, GetData));
    }

    

    public void GetData()
    {
        mode = PageMode.Create;
        
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.GET, getUrl, callback: loadData));
    }


    void clearObjects()
    {
        for (int i = 1; i < listParent.childCount; i++)
        {
            Destroy(listParent.GetChild(i).gameObject);
        }
    }

    RequiredDocsGetResponse response = new RequiredDocsGetResponse();

    void loadData(string json)
    {
        response = JsonUtility.FromJson<RequiredDocsGetResponse>(json);

        Debug.Log(response);

        if (response.data == null)
        {
            return;
        }

        clearObjects();

        mode = PageMode.Update;

        foreach (RequiredDocsGetField field in response.data.field)
        {
            GameObject obj = Instantiate(requiredDocsItem, listParent);
            obj.GetComponent<RequiredDocsItem>().PopulateRequiredDocsItem(field);
        }

        LoginMetaUI.Instance.Loader(false);
    }

    public void NextPage()
    {
        Debug.Log("Page mode : "+mode);
        isVisited = true;
        isSkipped = false;

        UpdateData();
    }

    public void BackPage()
    {
        GlobalData.RequiredDocsId = "";
        UIEventSystem.TabGroupChange(TabGroups.Dashboard);
    }

    public void SkipPage()
    {
        UIEventSystem.TabGroupChange(TabGroups.InspectionCriteria);
    }

    public void UpdateData()
    {
        RequiredDocsUpdateData requiredDocsUpdateData = new RequiredDocsUpdateData();

        requiredDocsUpdateData.productId = GlobalData.productID;

        List<RequiredDocsGetField> fields = new List<RequiredDocsGetField>();

        for (int i = 1; i < listParent.childCount; i++)
        {
            fields.Add(listParent.GetChild(i).GetComponent<RequiredDocsItem>().GetData());
        }

        requiredDocsUpdateData.field = fields;
        LoginMetaUI.Instance.Loader(true);
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.PUT, updateUrl, JsonUtility.ToJson(requiredDocsUpdateData), callback: recordResponse));
       
    }

    void recordResponse(string json)
    {
        ResponseMessage response = JsonUtility.FromJson<ResponseMessage>(json);
        LoginMetaUI.Instance.Loader(false);
        UIEventSystem.TabGroupChange(TabGroups.InspectionCriteria);
    }
   
}
