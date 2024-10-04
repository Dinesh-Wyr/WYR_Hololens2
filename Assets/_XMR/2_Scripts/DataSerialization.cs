using System;
using System.Collections.Generic;
using UnityEngine;

#region Geo Location

[Serializable]
public class GeoLocation
{
    public double latitude;
    public double longitude;
}

#endregion

#region Dashboard

[Serializable]
public class DashboardResponse
{
    public string message;
    public List<PLData> data;
}

[Serializable]
public class PLData
{
    public string packingId;
    public string packingList;
    public List<PurchaseOrder> purchaseOrders;
}

[Serializable]
public class PurchaseOrder
{
    public string poId;
    public string poNumber;
    public List<Product> products;
}

[Serializable]
public class Product
{
    public string productId;
    public string styleId;
    public string styleName;
    public string color;
    public string quantity;
    public string size;
    public string status;
}

[Serializable]
public class ProductID
{
    public string productId;
}

public class ResponseMessage
{
    public string message;
}

#endregion

#region Required Docs

[Serializable]
public class RequiredDocsGetResponse
{
    public string message;
    public RequiredDocsGetData data;

}

[Serializable]
public class RequiredDocsGetData
{
    public string _id;
    public string productId;
    public List<RequiredDocsGetField> field;
    public string isCreatedBy;
    public DateTime createdAt;
    public DateTime updatedAt;
    public int __v;
}

[Serializable]
public class RequiredDocsGetField
{
    public string document;
    public bool receivedClient;
    public bool receivedVendor;
    public bool NA;
    public string remarks;
    public string _id;
}

public class RequiredDocsUpdateData
{
    public string productId;
    public List<RequiredDocsGetField> field;
}

#endregion

#region Data Classes

[Serializable]
public class S3RequestData
{
    public string poid;
    public string plid;
    public string contentType;
    public string filename;

    public S3RequestData(string poid, string plid, string contentType, string filename)
    {
        this.poid = poid;
        this.plid = plid;
        this.contentType = contentType;
        this.filename = filename;
    }
}
[Serializable]
public class GetS3URLResponse
{
    public string message;
    public string url;
}

[System.Serializable]
public class S3PostData
{
    public string image;
    public string poid;
    public string plid;
    public string[] comments;

    public S3PostData(string image, string poid, string plid, string comments)
    {
        this.image = image;
        this.poid = poid;
        this.plid = plid;
        this.comments = new string[] { comments };
    }
}

[Serializable]
public class CartonCountResponse
{
    public string class_;
    public int count;
    public string preview;
}

[Serializable]
public class SendCommentsWithDefectType
{
    public string image;
    public string poid;
    public string plid;
    public List<string> comments;
    public string defectType;

    public SendCommentsWithDefectType(string image, string poid, string plid, List<string> comments, string defectType)
    {
        this.image = image;
        this.poid = poid;
        this.plid = plid;
        this.comments = comments;
        this.defectType = defectType;
    }
}

#endregion

public class DataSerialization : MonoBehaviour
{
   
}
