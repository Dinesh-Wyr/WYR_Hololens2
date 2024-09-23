using System;
using UnityEngine;

[Serializable]
public class RequiredDocsGetResponse
{
    public string message;
    public RequiredDocsResponse RDocCheck;
}

[Serializable]
public class RequiredDocsCreateResponse
{
    public string message;
    public RequiredDocsResponse RDocCheck;
}

[Serializable]
public class RequiredDocs
{
    public string poid;
    public string plid;
    public RequiredDocsRow purchaseOrder;
    public RequiredDocsRow measurementSpecification;
    public RequiredDocsRow referenceSample;
    public RequiredDocsRow QCFile;
    public RequiredDocsRow testReport;
    public RequiredDocsRow supplierInjectionReport;
    public RequiredDocsRow packingList;
    public RequiredDocsRow other;
    public bool isVisited;
    public bool skip;
}

[Serializable]
public class RequiredDocsParameter
{
    public string poid;
    public string plid;
    public RequiredDocsRow purchaseOrder;
    public RequiredDocsRow measurementSpecification;
    public RequiredDocsRow referenceSample;
    public RequiredDocsRow QCFile;
    public RequiredDocsRow testReport;
    public RequiredDocsRow supplierInjectionReport;
    public RequiredDocsRow packingList;
    public RequiredDocsRow other;
    public bool isVisited;
    public bool skip;
    public string _id;
}

[Serializable]
public class RequiredDocsResponse
{
    public string poid;
    public string plid;
    public RequiredDocsRow purchaseOrder;
    public RequiredDocsRow measurementSpecification;
    public RequiredDocsRow referenceSample;
    public RequiredDocsRow QCFile;
    public RequiredDocsRow testReport;
    public RequiredDocsRow supplierInjectionReport;
    public RequiredDocsRow packingList;
    public RequiredDocsRow other;
    public bool isVisited;
    public bool skip;
    public string isCreatedBy;
    public string _id;
    public string createdAt;
    public string updatedAt;
    public int __v;
}

public class RequiredDocsManager : MonoBehaviour
{
    [SerializeField]
    RequiredDocs doc;
    [SerializeField]
    RequiredDocsResponse docResponse;

    [Header("Doc Rows")]
    [SerializeField]
    RequiredDocsItem purchaseOrder;
    [SerializeField]
    RequiredDocsItem measurementSpecification;
    [SerializeField]
    RequiredDocsItem referenceSample;
    [SerializeField]
    RequiredDocsItem QCFile;
    [SerializeField]
    RequiredDocsItem testReport;
    [SerializeField]
    RequiredDocsItem supplierInjectionReport;
    [SerializeField]
    RequiredDocsItem packingList;
    [SerializeField]
    RequiredDocsItem other;
    [SerializeField]
    bool isVisited;
    [SerializeField]
    bool isSkipped;

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
        getUrl = GlobalData.ApiLink + APIEndpoints.Instance.getDocsEndPoint;
        storeUrl = GlobalData.ApiLink + APIEndpoints.Instance.addDocsEndPoint;
        updateUrl = GlobalData.ApiLink + APIEndpoints.Instance.updateDocsEndPoint;

        Debug.Log(getUrl);
        Debug.Log(storeUrl);
        Debug.Log(updateUrl);

        StartCoroutine(ApiCallUtility.Instance.DelayedCall(1, GetData));

        //GetData();

    }

    

    public void GetData()
    {
        mode = PageMode.Create;
        //Debug.Log("plid : " + GlobalData.plid + " , poid : " + GlobalData.poid);
        string json = "{\"poid\":\""+GlobalData.poid+"\",\"plid\":\""+GlobalData.plid+"\"}";
        Debug.Log(json);
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.GET, getUrl, json, callback: loadData));
    }

    void loadData(string json)
    {
        RequiredDocsGetResponse response = JsonUtility.FromJson<RequiredDocsGetResponse>(json);

        if (response.RDocCheck == null)
        {
            return;
        }

        mode = PageMode.Update;

        docResponse = response.RDocCheck;
        purchaseOrder.LoadData(docResponse.purchaseOrder);
        measurementSpecification.LoadData(docResponse.measurementSpecification);
        referenceSample.LoadData(docResponse.referenceSample);
        QCFile.LoadData(docResponse.QCFile);
        testReport.LoadData(docResponse.testReport);
        supplierInjectionReport.LoadData(docResponse.supplierInjectionReport);
        packingList.LoadData(docResponse.packingList);
        other.LoadData(docResponse.other);
        isVisited = docResponse.isVisited;
        isSkipped = docResponse.skip;
        GlobalData.RequiredDocsId = docResponse._id;
        
    }

    public void NextPage()
    {
        Debug.Log("Page mode : "+mode);
        isVisited = true;
        isSkipped = false;
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
        UIEventSystem.TabGroupChange(TabGroups.Dashboard);
    }

    public void SkipPage()
    {
        if(mode == PageMode.Create)
        {
            CreateData(true);
        }
        else
        {
            UIEventSystem.TabGroupChange(TabGroups.InspectionCriteria);
        }
    }

    public void CreateData(bool skip = false)
    {
        doc.poid = GlobalData.poid;
        doc.plid = GlobalData.plid;
        doc.purchaseOrder = purchaseOrder.GetData();
        doc.measurementSpecification = measurementSpecification.GetData();
        doc.referenceSample = referenceSample.GetData();
        doc.QCFile = QCFile.GetData();
        doc.testReport = testReport.GetData();
        doc.supplierInjectionReport = supplierInjectionReport.GetData();
        doc.packingList = packingList.GetData();
        doc.other = other.GetData();
        doc.isVisited = isVisited;
        doc.skip = skip;

        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, storeUrl, JsonUtility.ToJson(doc), callback: recordResponse));
    }

    public void UpdateData()
    {
        RequiredDocsParameter doc = new RequiredDocsParameter();
        doc.poid = GlobalData.poid;
        doc.plid = GlobalData.plid;
        doc.purchaseOrder = purchaseOrder.GetData();
        doc.measurementSpecification = measurementSpecification.GetData();
        doc.referenceSample = referenceSample.GetData();
        doc.QCFile = QCFile.GetData();
        doc.testReport = testReport.GetData();
        doc.supplierInjectionReport = supplierInjectionReport.GetData();
        doc.packingList = packingList.GetData();
        doc.other = other.GetData();
        doc.isVisited = isVisited;
        doc.skip = false;
        doc._id = docResponse._id;
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.PUT, updateUrl, JsonUtility.ToJson(doc), callback: recordResponse));
    }

    void recordResponse(string json)
    {
        RequiredDocsCreateResponse response = JsonUtility.FromJson<RequiredDocsCreateResponse>(json);
        //Debug.Log("response parse");
        Debug.Log(JsonUtility.ToJson(response));
        if (response.RDocCheck != null && mode == PageMode.Create)
        {
            GlobalData.RequiredDocsId = response.RDocCheck._id;
            Debug.Log(GlobalData.RequiredDocsId);
        }
        else
        {
            Debug.LogError("doc not parsed");
        }

        UIEventSystem.TabGroupChange(TabGroups.InspectionCriteria);
    }
   
}
