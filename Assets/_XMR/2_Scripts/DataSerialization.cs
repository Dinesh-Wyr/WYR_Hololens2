using System;
using System.Collections.Generic;
using UnityEngine;

#region DataSerialization

[System.Serializable]
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
