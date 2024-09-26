using System;
using UnityEngine;

public class APIEndpoints : MonoBehaviour
{
    public static APIEndpoints Instance;

    public static event Action baseURLSet;

    public enum SelectBaseURL
    {
        Production,
        Development,
        Local
    }

    public APIEndpointsScriptableObjectScript apiEndpointsObject;

    [SerializeField]
    SelectBaseURL selectedBaseURL;

    [HideInInspector]
    public string loginEndPoint, dashboardEndPoint, GetS3EndPoint, S3PostCommentsEndPoint, getDocsEndPoint, addDocsEndPoint, updateDocsEndPoint, getCriteriaEndPoint, addCriteriaEndPoint, updateCriteriaEndPoint, barcodeScannerEndPoint;

    // API endpoints
    [HideInInspector]
    public string GetCartonCountURL, map_user_nameEndPoint, GetUserNameEndpoint, ShareChannelEndPoint, EndInspectionEndPoint;

    // baseURL for all API's
    [HideInInspector]
    public string baseURL;

    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        
    }

    private void Start()
    {
        switch (selectedBaseURL)
        {
            case SelectBaseURL.Production:
                baseURL = apiEndpointsObject.productionBaseURl;
                break;

            case SelectBaseURL.Development:
                baseURL = apiEndpointsObject.developementBaseURL;
                break;
            case SelectBaseURL.Local:
                baseURL = apiEndpointsObject.localBaseURL;
                break;
        }

        GlobalData.ApiLink = baseURL;
        Debug.Log(GlobalData.ApiLink);
        LoginMetaUI.Instance.Log(GlobalData.ApiLink);



        loginEndPoint = apiEndpointsObject.loginEndPoint;
        dashboardEndPoint = apiEndpointsObject.dashboardEndPoint;
        GetS3EndPoint = apiEndpointsObject.GetS3EndPoint;
        S3PostCommentsEndPoint = apiEndpointsObject.S3PostCommentsEndPoint;
        getDocsEndPoint = apiEndpointsObject.getDocsEndPoint;
        addDocsEndPoint = apiEndpointsObject.addDocsEndPoint;
        updateDocsEndPoint = apiEndpointsObject.updateDocsEndPoint;
        getCriteriaEndPoint = apiEndpointsObject.getCriteriaEndPoint;
        addCriteriaEndPoint = apiEndpointsObject.addCriteriaEndPoint;
        updateCriteriaEndPoint = apiEndpointsObject.updateCriteriaEndPoint;
        barcodeScannerEndPoint = apiEndpointsObject.barcodeScannerEndPoint;


        GetCartonCountURL = apiEndpointsObject.GetCartonCountURL;
        map_user_nameEndPoint = apiEndpointsObject.map_user_nameEndPoint;
        GetUserNameEndpoint = apiEndpointsObject.GetUserNameEndpoint;
        ShareChannelEndPoint = apiEndpointsObject.ShareChannelEndPoint;
        EndInspectionEndPoint = apiEndpointsObject.EndInspectionEndPoint;

        baseURLSet?.Invoke();
    }

}
