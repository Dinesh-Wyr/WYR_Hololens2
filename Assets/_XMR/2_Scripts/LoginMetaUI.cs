using Microsoft.MixedReality.Toolkit.Experimental.UI;
using MixedReality.Toolkit.UX;
using System;
using TMPro;
using UnityEngine;

public class LoginMetaUI : MonoBehaviour
{

    [SerializeField] private GameObject MetaUI;
    [SerializeField] private TMP_InputField EmailInput;
    [SerializeField] private TMP_InputField PasswordInput;
    [SerializeField] private PressableButton SignIn;
    [SerializeField] private GameObject kb;
    [SerializeField] GameObject EndInspectionConfirmObj;
    public GameObject barcodeCameraButton;

    public static NonNativeKeyboard keyboard;
    public static PO_Item po_instance;
    public static TMP_InputField selected_field;
    public static LoginMetaUI Instance;

    [SerializeField] GameObject UILoader;


    [SerializeField]
    TextMeshProUGUI debugLogText;
    [SerializeField]
    GameObject debugLog;
    [SerializeField]
    Transform debugContent;


    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        // sign in button is not clickable until both fields have some value
        if (EmailInput.text.Length == 0 || PasswordInput.text.Length == 0)
        {
            SignIn.enabled = false;
        }
        else
        {
            SignIn.enabled = true;
        }

        // display keyboard field's text on the UI
        if (KeyboardManager.isKeyboardOn && selected_field != null && keyboard != null)
        {
            selected_field.text = keyboard.InputField.text;

        }
    }

    /// <summary>
    /// Open keyboard for an input field.
    /// </summary>
    /// <param name="field"> input field for which keyboard is opened </param>
    public void SelectField(TMP_InputField field)
    {
        if (field == null || KeyboardManager.isKeyboardOn)
            return;
        else
        {
            // KeyboardOpener.ipfield == 1 when standard input field is selected and 2 when password field is selected 
            _ = field.gameObject.name == "PasswordField" ? KeyboardManager.inputFieldType = InputFieldType.PASSWORD : KeyboardManager.inputFieldType = InputFieldType.STANDARD;
            Debug.Log("Open " + KeyboardManager.inputFieldType);
            selected_field = field;
        }

        if(kb!=null && !kb.activeSelf)
        kb.SetActive(true);

        if (keyboard == null)
        {
            keyboard = NonNativeKeyboard.Instance;
            
        }
        else
        {
            keyboard.Close();
            keyboard = NonNativeKeyboard.Instance;
        }

        KeyboardManager.isKeyboardOn = true;
        keyboard.PresentKeyboard();
       
        // set content type of field in keyboard
        if(KeyboardManager.inputFieldType == InputFieldType.STANDARD)
        {
            keyboard.InputField.contentType = TMP_InputField.ContentType.Standard;
        }
        else if(KeyboardManager.inputFieldType == InputFieldType.PASSWORD)
        {
            keyboard.InputField.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            keyboard.InputField.contentType = TMP_InputField.ContentType.Standard;
        }

       // keyboard.InputField.ActivateInputField();

        keyboard.InputField.text = field.text;

        keyboard.InputField.enabled = field.enabled = false;
        keyboard.InputField.enabled = field.enabled = true;


        keyboard.InputField.shouldHideSoftKeyboard = true;

        //field.caretPosition = field.text.Length;

        // buttons for saving , taking comments should be hidden
        KeyboardManager.Instance.voiceTypingButton.SetActive(false);
        KeyboardManager.Instance.closeButton.SetActive(true);

    }

    /// <summary>
    /// Take confirmation for end inspection 
    /// </summary>
    public void ShowEndInspectionConfirmation()
    {
        try
        {
            Debug.Log("Showing end inspection confirmation");
            EndInspectionConfirmObj.SetActive(true);
            //QuitApplication.instance.PositionObjectFrontOfCamera(EndInspectionConfirmObj, 0.2f);
        }
        catch (Exception e)
        {
            Debug.Log(" error showing End Inspection Confirmation..." + e);
        }
    }

    /// <summary>
    /// User continues inspection
    /// </summary>
    public void DontEndInspection()
    {
        try
        {
            EndInspectionConfirmObj.SetActive(false);
            Debug.Log("Continuing Inspection...");
        }
        catch (Exception e)
        {
            Debug.Log("Error continuing Inspection " + e);
        }
    }

    /// <summary>
    /// User ends inspection
    /// </summary>
    public void EndInspection()
    {
        EndInspectionConfirmObj.SetActive(false);
        if (string.IsNullOrEmpty(GlobalData.plid) || string.IsNullOrEmpty(GlobalData.poid))
        {
            return;
        }

        string jsonData = "{ \"poid\":\"" + GlobalData.poid + "\" , \"plid\":\"" + GlobalData.plid + "\" }";
        string url = GlobalData.ApiLink + APIEndpoints.Instance.EndInspectionEndPoint;
        LoginMetaUI.Instance.Loader(true);
        StartCoroutine(ApiCallUtility.Instance.APIRequest(Method.POST, url, jsonData, callback: EndInspectionResponse));
        GlobalData.poid = null;
        GlobalData.plid = null;
        GlobalData.plNumber = null;
        GlobalData.poNumber = null;
        UIEventSystem.EndInspection();
        //LivestreamUIManager.Instance.StopStreaming();
        barcodeCameraButton.SetActive(false);
        //CommunicationUIManager.Instance.LeaveChannel();
        UIEventSystem.TabGroupChange(TabGroups.Dashboard);

    }


    public void Loader(bool status)
    {
        if (status)
            UILoader.SetActive(true);
        else
            UILoader.SetActive(false);
    }

    public void Log(string debug, bool isError = false)
    {
        GameObject prefab = Instantiate(debugLog, debugContent);

        TextMeshProUGUI textProUGUI = prefab.GetComponent<TextMeshProUGUI>();

        if (!isError)
            textProUGUI.text += debug;
        else
            textProUGUI.text += "<color=red>" + debug + "</color>";
    }


    void EndInspectionResponse(string json)
    {
        LoginMetaUI.Instance.Loader(false);
        /*
        UserMappingResponse response = JsonUtility.FromJson<UserMappingResponse>(json);
        if (response != null)
        {
            Debug.Log("Response Not null " + JsonUtility.ToJson(response));
            string id = response.value._id;
            Debug.Log("id = " + id);
        }
        else
        {
            Debug.Log("Response NULL");
        }*/
    }


}


