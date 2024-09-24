using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine.UI;
using MixedReality.Toolkit.Examples.Demos;
using System;
#if WINDOWS_UWP
using Windows.UI.Text.Core;
#endif

public enum InputFieldType
{
    STANDARD,
    PASSWORD
}

// script to open keyboard when user wants to take comments.
public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance;

    public static bool isKeyboardOn;
    public static InputFieldType inputFieldType = InputFieldType.STANDARD;

    public bool isSaveComments;

    [Space]
    public GameObject voiceTypingButton;
    public GameObject closeButton;

    [Space]

    [SerializeField] DictationHandler dictationHandler;
    [SerializeField] private Image voiceTypingImage;
    [SerializeField] private Sprite voiceTypingOn;
    [SerializeField] private Sprite voiceTypingOff;
    [SerializeField] Color voiceTypingOnColor;
    [SerializeField] Color voiceTypingOffColor;

    

    private static TMP_InputField inputfield;
    private static string lastText;
    private static NonNativeKeyboard keyboard;
    private static bool voiceTyping = true;
    private static string lastUtterance;
 
    TMP_InputField currentInputField;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        NonNativeKeyboard.OnEnter += OnEnterPressed;
    }

    private void OnDisable()
    {
        NonNativeKeyboard.OnEnter -= OnEnterPressed;
    }

    void Start()
    {
#if WINDOWS_UWP
        CoreTextServicesManager manager = CoreTextServicesManager.GetForCurrentView();
        CoreTextEditContext _editContext = manager.CreateEditContext();
        _editContext.InputPaneDisplayPolicy = CoreTextInputPaneDisplayPolicy.Manual;
#endif
        SwitchVoiceTyping();
        onListening();

        isKeyboardOn = false;
        Debug.Log("Closed " + isKeyboardOn);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentInputField != null)
        {
            currentInputField.text = keyboard.InputField.text;
        }

    }

    void OnEnterPressed()
    {
        Debug.Log("OnEnterPressed");
        Debug.Log(isSaveComments);
        if (isSaveComments)
        {
            
            SaveCommentsManager.Instance.SaveComments();
        }
        else
        {
            closeKeyboard();
        }
    }

    /// <summary>
    /// present keyboard to user ,called when user "says take a note".
    /// </summary>
    public void OpenCommentsKeyboard()
    {
        Debug.Log("Opening Keyboard for taking notes....");
        if (!GlobalData.ScreenshotTaken)
        {
            Debug.Log("Take Screenshot First");
        }
        else if (!GlobalData.ScreenshotPreviewShown)
        {
            Debug.Log("Wait for Screenshot preview");
        }

        // if keyboard already opened
        if (keyboard != null)
        {
            closeKeyboard();
        }


        ShowVoiceEnabledKeyboard();

/*
        // means keyboard open for taking notes
        isKeyboardOn = true;
        Debug.Log("Comments " + isKeyboardOn);
        DictationHandler.Instance.recognized = "";

        keyboard = NonNativeKeyboard.Instance;
        keyboard.PresentKeyboard();
        keyboard.InputField.contentType = TMP_InputField.ContentType.Standard;
        //keyboard.InputField.DeactivateInputField();
        // show particular buttons for taking comments like voice typing button , save comments button.
        // function enables these buttons.        
        voiceTyping = true;
        SwitchVoiceTyping();
        voiceTypingButton.SetActive(true);
        closeButton.SetActive(false);

        // set inputfield for keyboard
        inputfield = keyboard.InputField;
        inputfield.text = "";
        inputfield.ActivateInputField();
        keyboard.InputField.caretPosition = 0;
        voiceTyping = false;*/
    }

    /// <summary>
    /// called when user presses enter of close buttons or enter on keyboard
    /// </summary>
    public void closeKeyboard()
    {
        Debug.Log("Closing keyboard....");
        isKeyboardOn = false;
        Debug.Log("Closed Button" + isKeyboardOn);
        if (keyboard == null && LoginMetaUI.keyboard == null)
        {
            return;
        }
        // check which keyboard is open
        // if keyboard for typing comments is open
        if (keyboard != null)
        {

            if (inputfield != null)
                inputfield = null;

            voiceTyping = true;
            // voice typing is diabled
            SwitchVoiceTyping();

            isKeyboardOn = false;

            Debug.Log("closing keyboard");
            // close keyboard
            keyboard.Close();
            keyboard = null;
            
        }
        else
        {
            // keyboard for login , etc. is open.
            LoginMetaUI.keyboard.Close();
            LoginMetaUI.keyboard = null;
            LoginMetaUI.selected_field = null;
        }
       
       
    }


    /// <summary>
    /// Switch voice typing.
    /// </summary>
    public void SwitchVoiceTyping()
    {
        voiceTyping = !voiceTyping;

        if (voiceTyping)
        {
            voiceTypingImage.sprite = voiceTypingOn;
            voiceTypingImage.color = voiceTypingOnColor;
            dictationHandler.StartRecognition();
            dictationHandler.recognized = "";
            Debug.Log("Voice Typing Enabled");
        }
        else
        {
            voiceTypingImage.sprite = voiceTypingOff;
            voiceTypingImage.color = voiceTypingOffColor;
            dictationHandler.StopRecognition();
            Debug.Log("Voice Typing Disabled");
        }

    }


    /// <summary>
    /// Add voice typing utterance to user comments and log user utterances
    /// </summary>
    public void onListening()
    {
        /*
        AppDictationExperience.TranscriptionEvents.OnFullTranscription.AddListener((transcription) =>
        {
            // check if voice typing is enabled , keyboard is not null and last word != user utterance to prevent multiple duplicate words being added.
            if (voiceTyping == true && inputfield != null && lastText != transcription)
            {
                if (transcription.ToLower() != "save comments")
                {
                    inputfield.text += " " + transcription;
                    inputfield.caretPosition = inputfield.text.Length;
                }

                Debug.Log("INPUT FIELD = " + inputfield.text);
                lastText = transcription;
            }


            // log user utteracnces
            if (transcription.Length > 0 && lastUtterance != transcription)
            {
                Debug.Log("Logging Utterance");
                Debug.Log("USER UTTERANCE : " + transcription);
                lastUtterance = transcription;
            }
        });

        // prevent overflow of comment
        if (inputfield != null && inputfield.text.Length > 10000)
            inputfield.text = "";
        */
    }

    /// <summary>
    /// close keyboard when user presses Save Comments.
    /// </summary>
    public void UploadComments()
    {
        if (!isKeyboardOn)
            return;
        //StartCoroutine(SaveComments(utterance.text)); 
        closeKeyboard();
        //comments.SetActive(false);
    }

    /// <summary>
    /// Clear all text on keyboard
    /// </summary>
    public void ClearText()
    {

        Debug.Log("ClearText");
        // check if keyboard for typing comments is opne
        if(keyboard != null)
        {
            Debug.Log(keyboard.InputField.name);
            keyboard.InputField.text = "";
        }
        // check if any other keyboard is open
        else if(LoginMetaUI.keyboard != null)
        {
            Debug.Log(LoginMetaUI.keyboard.InputField.name);
            LoginMetaUI.keyboard.InputField.text = "";
        }
    }

    public void ShowVoiceEnabledKeyboard(TMP_InputField currentField = null)
    {
        isKeyboardOn = true;

        DictationHandler.Instance.recognized = "";
        
        keyboard = NonNativeKeyboard.Instance;
        
        
        keyboard.PresentKeyboard();
        keyboard.InputField.contentType = TMP_InputField.ContentType.Standard;

        voiceTyping = true;
        SwitchVoiceTyping();
        voiceTypingButton.SetActive(true);
        closeButton.SetActive(false);

        // set inputfield for keyboard
        inputfield = keyboard.InputField;
        inputfield.text = "";
        inputfield.ActivateInputField();
        keyboard.InputField.caretPosition = 0;
        voiceTyping = false;

        currentInputField = currentField;

       
    }


}

