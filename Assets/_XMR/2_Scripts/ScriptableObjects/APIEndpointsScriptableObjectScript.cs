using UnityEngine;

[CreateAssetMenu(fileName = "APIs", menuName = "ScriptableObjects/APIEndpointScriptableObject", order = 1)]
public class APIEndpointsScriptableObjectScript : ScriptableObject
{
    // Base URL for production API's
    public string productionBaseURl = "https://api.wyrai.com/api/";

    // Base URL for development API's
    public string developementBaseURL = "https://api.dev.wyrai.com/api/";

    // Base URL for local API's
    public string localBaseURL = "http://192.168.1.46:5000/api/";

    public string GetCartonCountURL = "https://ml.dev.wyrai.com/carton/carton_counter_fs";

    [Space(10)]

    // API endpoints
    public string loginEndPoint = "registration/login";
    public string dashboardEndPoint = "meta_dashboard/dashboard_get";

    [Space(10)]
    public string startInspectionEndPoint = "metaQuest/dashboard/inspectionStart";
    public string GetS3EndPoint = "videoInspection/image_S3";
    public string S3PostCommentsEndPoint = "videoInspection/image_Url"; 
    public string getDocsEndPoint = "required_docs/docs_get";
    public string addDocsEndPoint = "required_docs/docs_add";
    public string updateDocsEndPoint = "required_docs/docs_update";
    public string getCriteriaEndPoint = "inspection_criteria/criteriaGet";
    public string addCriteriaEndPoint = "inspection_criteria/criteriaAdd";
    public string updateCriteriaEndPoint = "inspection_criteria/criteriaUpdate";

    [Space(10)]
    public string barcodeScannerEndPoint = "barcode/barcodeScanner";

    [Space(10)]
    public string uploadImageEndPoint = "colorMatch/colorMatchImageUpload";
    public string colorMatchEndPoint = "colorMatch/colorMatching";


    [Space(10)]
    public string map_user_nameEndPoint = "live/userChannel";
    public string GetUserNameEndpoint = "live/userChannel";
    public string ShareChannelEndPoint = "live/channel";
    public string EndInspectionEndPoint = "videoInspection/reportGenrate";

}
