using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;



public class BluetoothManager : MonoBehaviour
{
    private static BluetoothManager _instance;

    public static BluetoothManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BluetoothManager>();

            }

            return _instance;
        }
    }

    public string deviceAdd;
    public List<string> deviceNames;
    public TMP_InputField wifiname;
    public TMP_InputField password;
    public TMP_Dropdown deviceDropdown;
    public string receivedData;
    private bool isConnected;

    public LivestreamUIManager uimanager;

    public ScreenshotCategories screenshotCategory;

    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;

    
    // Start is called before the first frame update
    void Start()
    {
        InitBluetooth();
        isConnected = false;
        StartCoroutine(delay());
        GetPairedDevices();
        wifiname.text = "Wyrai";
        password.text = "Wyrai@1102";
    }

    #region BluetoothManagerPackage

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
    }

    // creating an instance of the bluetooth class from the plugin 
    public void InitBluetooth()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // Check BT and location permissions
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
            || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {

            Permission.RequestUserPermissions(new string[] {
                        Permission.CoarseLocation,
                            Permission.FineLocation,
                            "android.permission.BLUETOOTH_ADMIN",
                            "android.permission.BLUETOOTH",
                            "android.permission.BLUETOOTH_SCAN",
                            "android.permission.BLUETOOTH_ADVERTISE",
                             "android.permission.BLUETOOTH_CONNECT"
                    });

        }

        unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
        BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
    }

    // Start device scan
    public void StartScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StartScanDevices");
    }

    // Stop device scan
    public void StopScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StopScanDevices");
    }

    // This function will be called by Java class to update the scan status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ScanStatus(string status)
    {
        Toast("Scan Status: " + status);
    }

    // This function will be called by Java class whenever a new device is found,
    // and delivers the new devices as a string data="MAC+NAME"
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void NewDeviceFound(string data)
    {
        
    }

    // Get paired devices from BT settings
    public void GetPairedDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // This function when called returns an array of PairedDevices as "MAC+Name" for each device found
        string[] data = BluetoothConnector.CallStatic<string[]>("GetPairedDevices"); ;

        TMP_Dropdown.OptionDataList deviceList = new TMP_Dropdown.OptionDataList();
        deviceNames = new List<string>();

        foreach (string s in data)
        {
            Debug.Log(s.Contains("raspberrypi").ToString());
            if (s.Contains("raspberrypi"))
            {
                Debug.Log("\n" + s.Split('+')[0]);
                //deviceAdd = s.Split('+')[0];
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = s.Split('+')[0];
                deviceList.options.Add(optionData);
                deviceNames.Add(s.Split('+')[0]);
            }
        }

        deviceDropdown.options.Clear();
        deviceDropdown.options = deviceList.options;
        deviceDropdown.value = 0;
        deviceAdd = deviceNames[0];
    }

    public void SelectDevice()
    {
        deviceAdd = deviceNames[deviceDropdown.value];
        Debug.Log("new device = " + deviceAdd);
    }

    public void ReloadScene()
    {
        StopConnection();
        SceneManager.LoadScene("SampleScene");
    }

    // Start BT connect using device MAC address "deviceAdd"
    public void StartConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        BTloading();
    }

    async void BTloading()
    {
        Debug.Log("Connecting on : " + deviceAdd);
        await BTConnect();
        uimanager.ShowBTLoading();
        isConnecting = true;
        time = 0;
    }

    async Task BTConnect()
    {
        BluetoothConnector.CallStatic("StartConnection", deviceAdd.ToString().ToUpper());
    }

    public bool IsBluetoothConnected()
    {
        return isConnected;
    }

    // Stop BT connetion
    public void StopConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("StopConnection");
    }

    // This function will be called by Java class to update BT connection status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ConnectionStatus(string status)
    {
        Toast("Connection Status: " + status);
        isConnected = status == "connected";
    }

    float timeoutDuration = 3;
    float time = 0;
    bool isConnecting = false;
    private void Update()
    {
        if (isConnecting)
        {
            time += Time.deltaTime;
            if (time < timeoutDuration)
            {
                if (isConnected)
                {
                    time = 0;
                    isConnecting = false;
                }
            }
            else
            {
                time = 0;
                isConnecting = false;
                uimanager.ShowBTFailure("Connection timed out");
            }
        }
        
    }

    // This function will be called by Java class whenever BT data is received,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadData(string data)
    {
        Debug.Log("BT Stream: " + data);
        receivedData = data;
        dataProcessing(data);
    }

    // This function will be called by Java class to send Log messages,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadLog(string data)
    {
        Debug.Log(data);
    }


    // Function to display an Android Toast message
    public void Toast(string data)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("Toast", data);
    }

    #endregion

    #region BluetoothConnectionHandler

    // Write data to the connected BT device
    public void WriteData()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            Debug.Log("{\"name\":\"" + wifiname.text.ToString() + "\",\"password\":\"" + password.text.ToString() + "\"");
            BluetoothConnector.CallStatic("WriteData", "{\"name\":\"" + wifiname.text.ToString() + "\",\"password\":\"" + password.text.ToString() + "\"}");
            uimanager.InputLoading();
        }
    }

    void dataProcessing(string data)
    {
        Debug.Log(data);
        try
        {
            ServerStatus status = (ServerStatus)int.Parse(data);
            Debug.Log(status);
            switch (status)
            {
                case ServerStatus.BT_Connect:
                    uimanager.ShowBTSuccess("Bluetooth Connected");
                    uimanager.InputLoading();
                    break;
                case ServerStatus.BT_Disconnect:
                    uimanager.ShowBTFailure("Bluetooth not connected");
                    break;
                case ServerStatus.Wifi_Connect:
                    uimanager.InputSuccess("Raspberry Pi connected to wifi");
                    break;
                case ServerStatus.Wifi_Pass_Wrong:
                    uimanager.InputFailure("Wifi password incorrect");
                    break;
                case ServerStatus.Wifi_NotFound:
                    uimanager.InputFailure("Wifi not found");
                    break;
                case ServerStatus.Wifi_NotConnect:
                    uimanager.InputFailure("Wifi not connected");
                    break;
                default:
                    handleUnknownResponse(data);
                    break;

            }
        }
        catch
        {
            handleUnknownResponse(data);
        }
        
    }

    void handleUnknownResponse(string data)
    {
       // Debug.Log("unknown response");
        if (data.StartsWith("screenshot"))
        {
            Debug.Log(data.Split(":")[1]);

            switch (screenshotCategory)
            {
                case ScreenshotCategories.Normal:
                    ScreenshotManager.Instance.GetFilepathAndProcess(data.Split(":")[1]);
                    break;
                case ScreenshotCategories.Carton:
                case ScreenshotCategories.Barcode:
                    ScreenshotManager.Instance.GetFilepathAndProcess(data.Split(":")[1], screenshotCategory);
                    break;
                    
            }
        }
    }

    #endregion

    #region VoiceCallHandler

    public void SendAgoraInfo(string json)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            BluetoothConnector.CallStatic("WriteData", json);
        }
    }

    public void StopStreaming()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            BluetoothConnector.CallStatic("WriteData", "st");
        }
    }

    #endregion

    #region Screenshot_Carton_Barcode

    public void TakeScreenShot(string cameraMode)
    {
        BluetoothConnector.CallStatic("WriteData", cameraMode);
    }

    #endregion

    #region unused
    /*
    public void ShutdownPopup()
    {
        uimanager.AreYouSurePopup("Are you sure you want to shutdown?", ShutdownPi);
    }*/

    /*
    public void ShutdownPi()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            BluetoothConnector.CallStatic("WriteData", "sd");
        }
    }*/
    #endregion
}
