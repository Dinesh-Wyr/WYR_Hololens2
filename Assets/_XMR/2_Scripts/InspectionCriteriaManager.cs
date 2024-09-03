using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InspectionType
{
    public bool finalRandomInspection;
    public bool duringProductionInspection;
}

[Serializable]
public class InspectionLevel
{
    public bool first;
    public bool second;
    public bool third;
}

[Serializable]
public class InspectionCriteria
{
    public string poid;
    public string plid;
    public InspectionType inspectionType = new InspectionType();
    public InspectionLevel inspectionLevel = new InspectionLevel();
    public bool isVisited;
}

[Serializable]
public class InspectionCriteriaResponse
{
    public string poid;
    public string plid;
    public InspectionType inspectionType = new InspectionType();
    public InspectionLevel inspectionLevel = new InspectionLevel();
    public bool isVisited;
    public string _id;
    public string isCreatedBy;
    public string createdAt;
    public string updatedAt;
    public string __v;
    public string isUpdatedBy;
}

[Serializable]
public class InspectionCriteriaParameter
{
    public string _id;
    public string poid;
    public string plid;
    public InspectionType inspectionType = new InspectionType();
    public InspectionLevel inspectionLevel = new InspectionLevel();
    public bool isVisited;
}

[Serializable]
public class InspectionCriteriaGetResponse
{
    public string message;
    public InspectionCriteriaResponse InspectionoCriteriaCheck;
}

public class InspectionCriteriaManager : MonoBehaviour
{
    [SerializeField]
    InspectionCriteriaResponse inspectionCriteriaResponse;
    [SerializeField]
    InspectionCriteria inspectionCriteria;

    [Header("UI elements")]
    [SerializeField]
    Toggle finalRandomInspection;
    [SerializeField]
    Toggle duringProductionInspection;
    [SerializeField]
    Toggle first;
    [SerializeField]
    Toggle second;
    [SerializeField]
    Toggle third;
    [SerializeField]
    bool isVisited = true;

    [Space(10)]
    [SerializeField]
    PageMode mode;

    string getUrl;
    string storeUrl;
    string updateUrl;


    private void OnEnable()
    {
        SetURL();
    }

    private void SetURL()
    {
        getUrl = GlobalData.ApiLink + APIEndpoints.Instance.getCriteriaEndPoint;
        storeUrl = GlobalData.ApiLink + APIEndpoints.Instance.addCriteriaEndPoint;
        updateUrl = GlobalData.ApiLink + APIEndpoints.Instance.updateCriteriaEndPoint;

        Debug.Log(getUrl);
        Debug.Log(storeUrl);
        Debug.Log(updateUrl);

        StartCoroutine(ApiCallUtility.Instance.DelayedCall(1, GetData));

    }



    public void GetData()
    {
        mode = PageMode.Create;
        //Debug.Log("plid : " + GlobalData.plid + " , poid : " + GlobalData.poid);
        string json = "{\"poid\":\"" + GlobalData.poid + "\",\"plid\":\"" + GlobalData.plid + "\"}";
        Debug.Log(json);
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.GET, getUrl, json,callback: loadData));
    }

    void loadData(string json)
    {
        InspectionCriteriaGetResponse response = JsonUtility.FromJson<InspectionCriteriaGetResponse>(json);

        if (response.InspectionoCriteriaCheck == null)
        {
            return;
        }

        mode = PageMode.Update;

        inspectionCriteriaResponse = response.InspectionoCriteriaCheck;
        
        GlobalData.RequiredDocsId = inspectionCriteriaResponse._id;

        finalRandomInspection.isOn = inspectionCriteriaResponse.inspectionType.finalRandomInspection;
        duringProductionInspection.isOn = inspectionCriteriaResponse.inspectionType.duringProductionInspection;

        first.isOn = inspectionCriteriaResponse.inspectionLevel.first;
        second.isOn = inspectionCriteriaResponse.inspectionLevel.second;
        third.isOn = inspectionCriteriaResponse.inspectionLevel.third;

    }

    public void NextPage()
    {
        if (mode == PageMode.Create)
        {
            CreateData();
        }
        else if (mode == PageMode.Update)
        {
            UpdateData();
        }
    }

    public void BackPage()
    {
        GlobalData.RequiredDocsId = "";
        UIEventSystem.TabGroupChange(TabGroups.RequiredDocs);
    }

    public void CreateData()
    {
        inspectionCriteria.poid = GlobalData.poid;
        inspectionCriteria.plid = GlobalData.plid;
        inspectionCriteria.inspectionType.finalRandomInspection = finalRandomInspection.isOn; ;
        inspectionCriteria.inspectionType.duringProductionInspection = duringProductionInspection.isOn;
        inspectionCriteria.inspectionLevel.first = first.isOn;
        inspectionCriteria.inspectionLevel.second = second.isOn;
        inspectionCriteria.inspectionLevel.third = third.isOn;

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, storeUrl, JsonUtility.ToJson(inspectionCriteria),callback: recordResponse));

    }

    public void UpdateData()
    {
        InspectionCriteriaParameter inspectionCriteriaParameter = new InspectionCriteriaParameter();
        inspectionCriteriaParameter._id = inspectionCriteriaResponse._id;
        inspectionCriteriaParameter.poid = GlobalData.poid;
        inspectionCriteriaParameter.plid = GlobalData.plid;
        inspectionCriteriaParameter.inspectionType.finalRandomInspection = finalRandomInspection.isOn;
        inspectionCriteriaParameter.inspectionType.duringProductionInspection= duringProductionInspection.isOn;
        inspectionCriteriaParameter.inspectionLevel.first= first.isOn;
        inspectionCriteriaParameter.inspectionLevel.second= second.isOn;
        inspectionCriteriaParameter.inspectionLevel.third= third.isOn;

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.PUT, updateUrl, JsonUtility.ToJson(inspectionCriteriaParameter), callback: recordResponse));
    }

    void recordResponse(string json)
    {
        InspectionCriteriaGetResponse response = JsonUtility.FromJson<InspectionCriteriaGetResponse>(json);
        //Debug.Log("response parse");
        Debug.Log(JsonUtility.ToJson(response));
        if (response.InspectionoCriteriaCheck != null && mode == PageMode.Create)
        {
            GlobalData.RequiredDocsId = response.InspectionoCriteriaCheck._id;
            Debug.Log(GlobalData.RequiredDocsId);
        }
        else
        {
            Debug.LogError("doc not parsed");
        }

        UIEventSystem.TabGroupChange(TabGroups.InspectionCriteria);
    }

   
}
