using TMPro;
using UnityEngine;

public class ProductItem : MonoBehaviour
{
    [SerializeField] Product product;
    [SerializeField] string plid;
    [SerializeField] string pl_number;
    [SerializeField] string poid;
    [SerializeField] string po_number;

    [SerializeField] TextMeshProUGUI styleIDText;
    [SerializeField] TextMeshProUGUI styleNameText;
    [SerializeField] TextMeshProUGUI colorText;
    [SerializeField] TextMeshProUGUI sizeText;

    [SerializeField] GameObject StartInspectionBtn;
    [SerializeField] GameObject RescheduleBtn;
    [SerializeField] GameObject EndInspectionBtn;
    [SerializeField] GameObject PauseInspectionBtn;
    [SerializeField] GameObject EndInspectionConfirmObj;

    //[SerializeField] GameObject RescheduleConfirmObj;

    private static GameObject EndInsConfirmation;

    public void PopulateProductData(Product product, string plid, string pl_number, string poid, string po_number)
    {
        this.product = product;
        this.plid = plid;
        this.pl_number = pl_number;
        this.poid = poid;
        this.po_number = po_number;

        styleIDText.text = product.styleId;
        styleNameText.text = product.styleName;
        colorText.text = product.color;
        sizeText.text = product.size;

        if (GlobalData.poid == poid && GlobalData.plid == plid && GlobalData.productID == product.productId)
        {
            Debug.Log("Already");
            ViewInspectionStartedUI();
        }

    }

    /// <summary>
    /// Initalize variables while starting inspection
    /// </summary>
    public void StartInspection()
    {
        Debug.Log(string.IsNullOrEmpty(GlobalData.plid));
        Debug.Log(string.IsNullOrEmpty(GlobalData.poid));
        Debug.Log(string.IsNullOrEmpty(GlobalData.productID));

        if (!string.IsNullOrEmpty(GlobalData.plid) 
            || !string.IsNullOrEmpty(GlobalData.poid)
            || !string.IsNullOrEmpty(GlobalData.productID))
        {
            Debug.Log("inspection already going on");
            return;
        }

        GlobalData.poid = poid;
        GlobalData.plid = plid;
        GlobalData.poNumber = po_number;
        GlobalData.plNumber = pl_number;
        GlobalData.productID = product.productId;


        ProductID productID = new ProductID();
        productID.productId = GlobalData.productID;
        string url = GlobalData.ApiLink + APIEndpoints.Instance.startInspectionEndPoint;
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, url, JsonUtility.ToJson(productID), callback: StartInspectionResponse));

    }

    void StartInspectionResponse(string response)
    {
        ResponseMessage responseMessage = JsonUtility.FromJson<ResponseMessage>(response);

        if (!responseMessage.message.Equals("Inspection started successfully"))
            return;

        UIEventSystem.TabGroupChange(TabGroups.RequiredDocs);
        Debug.Log("Got Po/Pl id");
        ViewInspectionStartedUI();
        UIEventSystem.StartInspection();
        LoginMetaUI.productInstance = this;
    }


    public void ViewInspectionStartedUI()
    {
        EndInspectionBtn.SetActive(true);
        PauseInspectionBtn.SetActive(true);
        RescheduleBtn.SetActive(false);
        StartInspectionBtn.SetActive(false);

        LoginMetaUI.Instance.barcodeCameraButton.SetActive(true);
        LoginMetaUI.Instance.colorMatchButton.SetActive(true);
    }

    /// <summary>
    /// Show End Inspection Confirmation
    /// </summary>
    public void EndInspectionConfirmation()
    {
        LoginMetaUI.Instance.ShowEndInspectionConfirmation();
    }
}
