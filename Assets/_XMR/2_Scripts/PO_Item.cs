using System;
using TMPro;
using UnityEngine;

[Serializable]
public class PurchaseOrder
{
    public string _id;
    public string PO_Number;
}

public class PO_Item : MonoBehaviour
{
    [SerializeField] TMP_Text PO_number_Text;
    [SerializeField] PurchaseOrder purchaseOrder;
    [SerializeField] string plid;
    [SerializeField] GameObject StartInspectionBtn;
    [SerializeField] GameObject EndInspectionBtn;
    [SerializeField] GameObject RescheduleBtn;
    [SerializeField] GameObject EndInspectionConfirmObj;
    private static GameObject EndInsConfirmation;
    //private const string EndInspectionURL = "https://api.wyrai.com/api/videoInspection/reportGenrate";

    string pl_Number;

    public void PopulatePOData(PurchaseOrder purchaseOrder,string plid, string pl_number)
    {
        this.purchaseOrder = purchaseOrder;
        PO_number_Text.text = "PO Number : "+this.purchaseOrder.PO_Number;
        this.plid = plid;
        this.pl_Number= pl_number;

        if(GlobalData.poid == purchaseOrder._id &&  GlobalData.plid == plid)
        {
            ViewInspectionStartedUI();
        }
    }
    /// <summary>
    /// Initalize variables while starting inspection
    /// </summary>
    public void StartInspection()
    {
        if (!(string.IsNullOrEmpty(GlobalData.plid)) || (!string.IsNullOrEmpty(GlobalData.poid)))
        {
            Debug.Log("inspection already going on");
            return;
        }

        GlobalData.poid = purchaseOrder._id;
        GlobalData.plid = plid;
        GlobalData.poNumber = purchaseOrder.PO_Number;
        GlobalData.plNumber = pl_Number;
        UIEventSystem.TabGroupChange(TabGroups.RequiredDocs);
        Debug.Log("Got Po/Pl id");
        // SendMessageOverBridge.instance.SendPoPl();
        ViewInspectionStartedUI();
        UIEventSystem.StartInspection();
        LoginMetaUI.po_instance = this;
    }

    public void ViewInspectionStartedUI()
    {
        EndInspectionBtn.SetActive(true);
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
