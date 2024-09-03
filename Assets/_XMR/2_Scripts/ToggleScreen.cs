using UnityEngine;

public class ToggleScreen : MonoBehaviour
{
    [SerializeField] private GameObject Screen;
    [SerializeField] private GameObject UIScreen;
    private Canvas UICanvas;
    private MeshRenderer ScreenRenderer;

    /// <summary>
    /// Get Components from objects
    /// </summary>
    public void Awake()
    {
        UICanvas = UIScreen.GetComponent<Canvas>();
        ScreenRenderer = Screen.GetComponent<MeshRenderer>();
        Debug.Log("Got Components");
    }

    /// <summary>
    /// Show UI Screen when user says "Show Screen"
    /// </summary>
    public void ShowScreen()
    {
        if(UICanvas != null && ScreenRenderer != null)
        {
            ScreenRenderer.enabled = true;
            UICanvas.enabled = true;
            Debug.Log("Showing UI");

            Debug.Log("SUCCESS Showing UI");
        }
        else
        {
            Debug.Log("NULL COMPONENTS!!!");
            Debug.Log("ERROR Showing UI");
            if (UICanvas == null)
            {
                Debug.Log("UI null");
            }
            if (ScreenRenderer == null)
            {
                Debug.Log("Mesh Renderer null");
            }
        }
    }

    /// <summary>
    /// Hide UI Screen when user says Hide Screen
    /// </summary>
    public void HideScreen()
    {
        if (UICanvas != null && ScreenRenderer != null)
        {
            Debug.Log("Hiding UI");
            ScreenRenderer.enabled = false;
            UICanvas.enabled = false;
            Debug.Log("SUCCESS Hiding UI");
        }
        else
        {
            Debug.Log("NULL COMPONENTS!!!");
            Debug.Log("ERROR Hiding UI");
            if (UICanvas == null)
            {
                Debug.Log("UI null");
            }
            if (ScreenRenderer == null)
            {
                Debug.Log("Mesh Renderer null");
            }
        }
    }
}
