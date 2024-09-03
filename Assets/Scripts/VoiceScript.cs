using System.Collections;
using UnityEngine;
using Oculus.Voice;
using System.Text;
using TMPro;
using UnityEngine.Networking;
using Oculus.Voice.Dictation;
using UnityEngine.UI;


// Script to enable voice commands (update function)
public class VoiceScript : MonoBehaviour
{
    public AppVoiceExperience voiceExperience;
    public AppDictationExperience voiceDictation;
    public static bool dictation = false;

    public void PostComment()
    {
        dictation = true;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            // activating voice commands
            voiceExperience.Activate();
            // used for voice typing
            voiceDictation.Activate();
            yield return new WaitForEndOfFrame();
        }
    }
}