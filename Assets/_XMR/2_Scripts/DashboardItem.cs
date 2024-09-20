using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class PLData
{
    public string _id;
    public string Packing_List;
    public string time;
    public PurchaseOrder[] PurchaseOrders;
}

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
        packingListText.text = "Packing List : " + data.Packing_List;
        timeText.text = data.time;
        foreach(PurchaseOrder order in data.PurchaseOrders)
        {
            GameObject obj = Instantiate(PO_ListObject,PO_ListParent);
            obj.GetComponent<PO_Item>().PopulatePOData(order,plData._id,data.Packing_List);
        }
    }
}
