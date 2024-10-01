using TMPro;
using UnityEngine;

public class DashboardItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text packingListText;
    [SerializeField]
    TMP_Text timeText;
    [SerializeField]
    GameObject PO_ListObject;
    [SerializeField]
    Transform PO_ListParent;
    [SerializeField]
    PLData plData;

    public void PopulateDashboardItemInfo(PLData data)
    {
        plData = data;
        packingListText.text = "Packing List : " + data.packingList;
        foreach(PurchaseOrder order in data.purchaseOrders)
        {
            GameObject obj = Instantiate(PO_ListObject,PO_ListParent);
            obj.GetComponent<PO_Item>().PopulatePOData(order,plData.packingId, data.packingList);
        }
    }
}
