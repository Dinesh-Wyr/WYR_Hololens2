using TMPro;
using UnityEngine;

public class PO_Item : MonoBehaviour
{
    [SerializeField] TMP_Text PO_number_Text;
    [SerializeField] PurchaseOrder purchaseOrder;
    [SerializeField] string plid;

    [SerializeField]
    GameObject productObject;
    [SerializeField]
    Transform productParent;

    string pl_Number;

    public void PopulatePOData(PurchaseOrder purchaseOrder,string plid, string pl_number)
    {
        this.purchaseOrder = purchaseOrder;
        PO_number_Text.text = "PO Number : "+this.purchaseOrder.poNumber;
        this.plid = plid;
        this.pl_Number= pl_number;

        foreach (Product product in purchaseOrder.products)
        {
            GameObject obj = Instantiate(productObject, productParent);
            obj.GetComponent<ProductItem>().PopulateProductData(product, plid, pl_number, purchaseOrder.poId, purchaseOrder.poNumber);
        }
    }
}
