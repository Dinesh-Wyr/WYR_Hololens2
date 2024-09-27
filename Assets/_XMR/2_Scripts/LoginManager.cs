using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class LoginManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField emailPhone;
    [SerializeField]
    TMP_InputField password;
    [SerializeField]
    GameObject loginButton;
    [SerializeField]
    GameObject loader;
   
    
    public GameObject VoiceChatIcon;

    private static string uri;
    public static LoginManager instance;
    class LoginData
    {
        public string message;
        public string token;
        public string companyId;
    }
    public void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        APIEndpoints.baseURLSet += SetURL;
    }

    private void OnDisable()
    {
        APIEndpoints.baseURLSet -= SetURL;
    }

    private void SetURL()
    {
        uri = GlobalData.ApiLink + APIEndpoints.Instance.loginEndPoint;
        Debug.Log(uri);
        
    }

    public void Login()
    {
        StartCoroutine(PostForm(uri));
    }

    IEnumerator PostForm(string uri)
    {

        Debug.Log("TRYING TO Login");
        Debug.Log("Url = " + uri);
        WWWForm form = new();
        string email = emailPhone.text.ToString();
        string password = this.password.text.ToString();
        form.AddField("email", email);
        form.AddField("password", password);

        using UnityWebRequest www = UnityWebRequest.Post(uri, form);

        loginButton.SetActive(false);
        loader.SetActive(true);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            loginButton.SetActive(true);
            loader.SetActive(false);
            Debug.Log("ERROR Logging In");
            this.password.text = "";
        }
        else
        {
            

            Debug.Log(www.downloadHandler.text);
            LoginData data = JsonUtility.FromJson(www.downloadHandler.text, typeof(LoginData)) as LoginData;
            GlobalData.Token = data.token;
            Debug.Log(GlobalData.Token);
            GlobalData.IsLoggedIn = true;
            Debug.Log("SUCCESS Logging In");
           
            OnLoginSuccess();

        }
    }


    void OnLoginSuccess()
    {
        UIEventSystem.Login();
        UIEventSystem.TabGroupChange(TabGroups.Dashboard);
        password.text = "";
    }


    public void Logout()
    {
        Debug.Log("LOGGING OUT... ");
        UIEventSystem.Logout();

        GlobalData.Token = null;
        GlobalData.IsLoggedIn = false;

        loginButton.SetActive(true);
        loader.SetActive(false);
        UIEventSystem.TabGroupChange(TabGroups.Login);

    }
}