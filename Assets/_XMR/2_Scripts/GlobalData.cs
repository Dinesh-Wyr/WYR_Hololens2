
public enum PageMode
{
    Create,
    Update,
}

public static class GlobalData
{
    public static string ApiLink;

    public static string Token;
    public static bool IsLoggedIn;

    public static GeoLocation geoLocation;

    public static string Latitude;
    public static string Longitude;

    public static string poid;
    public static string plid;

    public static string poNumber;
    public static string plNumber;

    public static string productID;


    public static string RequiredDocsId;
    public static string InspectionCriteriaId;

    public static bool ScreenshotTaken = false;

    public static bool ScreenshotPreviewShown = false;

    public static bool VoiceCommandsActivate = false;
}
