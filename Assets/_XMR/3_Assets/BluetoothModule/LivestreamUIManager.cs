using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public enum ServerStatus
{
    BT_Connect,
    BT_Disconnect,
    Wifi_Connect,
    Wifi_NotConnect,
    Wifi_NotFound,
    Wifi_Pass_Wrong,
    Start_Stream,
    Stream_Start_failed,
    Stop_Stream
}

public enum CameraModes
{
    External,
    Internal
}

public enum ScreenshotCategories
{
    Normal,
    Carton,
    Barcode
}

public class LivestreamUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject BT_ConnectButton;
    [SerializeField]
    GameObject BT_ConnectionText;
    [SerializeField]
    GameObject BT_Loader;
    [SerializeField]
    GameObject BT_Dropdown;
    [SerializeField]
    GameObject BT_ScanButton;
    [SerializeField]
    TMP_Dropdown deviceDropdown;

    [Space(10)]
    [SerializeField]
    GameObject InputGroup;
    [SerializeField]
    GameObject InputLoader;
    [SerializeField]
    GameObject InputResponse;
    [SerializeField]
    TMP_InputField wifiname;
    [SerializeField]
    TMP_InputField password;

    [Space(10)]
    string deviceName;
    string dataToSend;
    public bool IsConnected
    {
        get
        {
            return BluetoothService.IsConnected();
        }
    }
    public static string dataRecived = "";
    public List<string> deviceNames;
    public ScreenshotCategories screenshotCategory;

    private static LivestreamUIManager _instance;

    public static LivestreamUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LivestreamUIManager>();

            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
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
        BluetoothService.CreateBluetoothObject();
        GetPairedDevices();
    }

    float timeoutDuration = 3;
    float time = 0;
    bool isConnecting = false;
    // Update is called once per frame
    void Update()
    {/*
        if (isConnecting)
        {
            time += Time.deltaTime;
            if (time < timeoutDuration)
            {
                if (IsConnected)
                {
                    time = 0;
                    isConnecting = false;
                }
            }
            else
            {
                time = 0;
                isConnecting = false;
                ShowBTFailure("Connection timed out");
            }
        }

        if (IsConnected)
        {
            try
            {
                string datain = BluetoothService.ReadFromBluetooth();
                if (datain.Length > 0)
                {
                    dataRecived = datain;
                    Debug.Log("BT data : " + datain);
                    dataProcessing(dataRecived);
                }

            }
            catch (Exception e)
            {
                BluetoothService.Toast("Error in connection");
            }
        }
*/
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
                    ShowBTSuccess("Bluetooth Connected");
                    InputLoading();
                    break;
                case ServerStatus.BT_Disconnect:
                    ShowBTFailure("Bluetooth not connected");
                    break;
                case ServerStatus.Wifi_Connect:
                    InputSuccess("Raspberry Pi connected to wifi");
                    break;
                case ServerStatus.Wifi_Pass_Wrong:
                    InputFailure("Wifi password incorrect");
                    break;
                case ServerStatus.Wifi_NotFound:
                    InputFailure("Wifi not found");
                    break;
                case ServerStatus.Wifi_NotConnect:
                    InputFailure("Wifi not connected");
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

    public void GetPairedDevices()
    {
        string[] devices = BluetoothService.GetBluetoothDevices();

        TMP_Dropdown.OptionDataList deviceList = new TMP_Dropdown.OptionDataList();
        deviceNames = new List<string>();

        foreach (var d in devices)
        {
            //Debug.Log(d.Contains("raspberrypi").ToString());
            //if (d.Contains("raspberrypi"))
            //{
            //    //Debug.Log("\n" + d.Split('i')[1]);
            //    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            //    optionData.text = d;
            //    deviceList.options.Add(optionData);
            //    deviceNames.Add(d);
            //}
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = d;
            deviceList.options.Add(optionData);
            deviceNames.Add(d);
        }

        deviceDropdown.options.Clear();
        deviceDropdown.options = deviceList.options;
        deviceDropdown.value = 0;
        deviceName = deviceNames[0];
    }

    public void SelectDevice()
    {
        deviceName = deviceNames[deviceDropdown.value];
        Debug.Log("new device = " + deviceName);
    }

    public void StartConnection()
    {
        if (!IsConnected)
        {
            ShowBTLoading();
            isConnecting = true;
            time = 0;
            StartCoroutine(StartBTConnection());
        }
    }

    IEnumerator StartBTConnection()
    {
        Debug.Log("Connection started");
        BluetoothService.StartBluetoothConnection(deviceName.ToString());
        yield return IsConnected;
        Debug.Log("Connection status : " + IsConnected);
        BluetoothService.Toast(deviceName.ToString() + " status: " + IsConnected);
    }

    public void SendData()
    {
        if (IsConnected && (dataToSend.ToString() != "" || dataToSend.ToString() != null))
            BluetoothService.WritetoBluetooth(dataToSend.ToString());
        else
            BluetoothService.WritetoBluetooth("Not connected");
    }

    #region VoiceCallHandler

    public void SendAgoraInfo(string json)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (IsConnected)
        {
            BluetoothService.WritetoBluetooth(json);
        }
    }

    public void StopStreaming()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (IsConnected)
        {
            BluetoothService.WritetoBluetooth("st");
        }
    }

    #endregion

    #region Screenshot_Carton_Barcode

    public void TakeScreenShot(string cameraMode)
    {
        BluetoothService.WritetoBluetooth(cameraMode);
    }

    #endregion

    public void SendWifiData()
    {
        BluetoothService.WritetoBluetooth("{\"name\":\"" + wifiname.text.ToString() + "\",\"password\":\"" + password.text.ToString() + "\"}");
    }

    public void ShowBTLoading()
    {
        BT_ConnectionText.SetActive(false);
        BT_ConnectButton.SetActive(false);
        BT_Loader.SetActive(true);
        BT_ScanButton.SetActive(false);
        BT_Dropdown.SetActive(false);
    }

    public void ShowBTSuccess(string text)
    {
        BT_ConnectButton.SetActive(false);
        BT_Loader.SetActive(false);
        BT_ConnectionText.SetActive(true);
        BT_ConnectionText.GetComponent<TMPro.TMP_Text>().text = text;
        BT_ScanButton.SetActive(false);
        BT_Dropdown.SetActive(false);
    }

    public void ShowBTFailure(string text) 
    {
        BT_ConnectButton.SetActive(true);
        BT_Loader.SetActive(false);
        BT_ConnectionText.SetActive(true);
        BT_ScanButton.SetActive(true);
        BT_Dropdown.SetActive(true);
        BT_ConnectionText.GetComponent<TMPro.TMP_Text>().text = text;
    }

    public void InputLoading()
    {
        InputGroup.SetActive(false);
        InputLoader.SetActive(true);
        InputResponse.SetActive(false);
    }

    public void InputSuccess(string text)
    {
        InputGroup.SetActive(false);
        InputLoader.SetActive(false);
        InputResponse.SetActive(true);
        InputResponse.GetComponent<TMPro.TMP_Text>().text=text;
    }

    public void InputFailure(string text)
    {
        InputGroup.SetActive(true);
        InputLoader.SetActive(false);
        InputResponse.SetActive(true);
        InputResponse.GetComponent<TMPro.TMP_Text>().text = text;
    }

    public void UnknownResponse(string text)
    {
        //defaultResponse.text = text;
    }

    public void OpenDashboard()
    {
        UIEventSystem.TabGroupChange(TabGroups.Dashboard);
    }
}
