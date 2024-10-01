using System;
using System.Collections;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Devices.Geolocation;
#endif

public class Geolocation : MonoBehaviour
{
#if WINDOWS_UWP
    private Geolocator geolocator;
#endif

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

#if WINDOWS_UWP
        geolocator = new Geolocator();
        geolocator.DesiredAccuracy = PositionAccuracy.High;
#endif
        GetLocation();
    }

    async void GetLocation()
    {

#if UNITY_EDITOR

        GeoLocation location = new GeoLocation();

        location.latitude = 28.692698704831443;
        location.longitude = 77.15191219147117;

        GlobalData.geoLocation = location;

        Debug.Log("Latitude: " + GlobalData.geoLocation.latitude);
        Debug.Log("Latitude: " + GlobalData.geoLocation.longitude);
#endif

#if WINDOWS_UWP
        try
        {

            Geoposition geoPosition = await geolocator.GetGeopositionAsync();

            GeoLocation location = new GeoLocation();

            location.latitude = geoPosition.Coordinate.Point.Position.Latitude;
            location.longitude = geoPosition.Coordinate.Point.Position.Longitude;

            GlobalData.geoLocation = location;

/*
            GlobalData.geoLocation.latitude = geoPosition.Coordinate.Point.Position.Latitude;
            GlobalData.geoLocation.longitude = geoPosition.Coordinate.Point.Position.Longitude;
*/

            
            Debug.Log("Latitude: " + GlobalData.geoLocation.latitude);
            Debug.Log("Longitude: " + GlobalData.geoLocation.longitude);


        }
        catch (Exception ex)
        {
            Debug.LogError("Error getting geolocation: " + ex.Message);
        }
#endif
    }

}