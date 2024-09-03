using System;
using UnityEngine;


public class BluetoothService
{

    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject activity;
    private static AndroidJavaObject context;
    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;


    // creating an instance of the bluetooth class from the plugin 
    public static void CreateBluetoothObject()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = activity.Call<AndroidJavaObject>("getApplicationContext");
            unity3dbluetoothplugin = new AndroidJavaClass("ai.wyr.bluetoothconnector.BluetoothConnector");
            BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
        }
    }

    public static string[] GetBluetoothDevices()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                return BluetoothConnector.Call<string[]>("GetBluetoothDevices");
            }
            catch (Exception e)
            {
                Toast("No Device found");
                return null;
            }
        }

        return null;

    }

    // starting bluetooth connection with device named "DeviceName"
    // print the status on the screen using native android Toast
    public static void StartBluetoothConnection(string DeviceName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                BluetoothConnector.Call("StartBluetoothConnection", DeviceName);

            }
            catch (Exception e)
            {
                Toast("Start connection error");
            }
        }
    }

    public static bool IsConnected()
    {
        return BluetoothConnector.GetStatic<bool>("IsConnected");
    }

    // should be called inside OnApplicationQuit
    // stop connection with the bluetooth device
    public static void StopBluetoothConnection()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                BluetoothConnector.Call("StopBluetoothConnection");
                Toast("Connction stoped");

            }
            catch (Exception e)
            {
                Toast("Stop connction error");
            }
        }
    }

    // write data as a string to the bluetooth device
    public static void WritetoBluetooth(string data)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                BluetoothConnector.Call("WriteData", data);
            }
            catch (Exception e)
            {
                Toast("Write data error");
            }
        }
    }


    //read data from the bluetooth device
    // if there is an error or there is no data coming, this method will return "" as an output
    public static string ReadFromBluetooth()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                return BluetoothConnector.Call<string>("ReadData");
            }
            catch (Exception e)
            {
                BluetoothConnector.Call("PrintOnScreen", context, "Read data error");
            }
        }
         
        return "";

    }

    public static void Toast(string data)
    {
        BluetoothConnector.Call("PrintOnScreen", context, data);
        Debug.Log(data);
    }

}
