using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonReplacer : EditorWindow
{
    [MenuItem("MRTK/Button Replacer")]
    static void Init()
    {
        GetWindow<ButtonReplacer>().Show();
    }

    List<GameObject> allButtons = new List<GameObject>();

    private void OnGUI()
    {
        GUILayout.Label("Select multiple GameObjects with UI Button components:");

        // Check if any GameObjects are selected
        if (Selection.objects.Length == 0)
        {
            GUILayout.Label("Please select GameObjects to replace buttons.");
            return;
        }

        
        //GUILayout.Button("Replace All");

        allButtons.Clear();

        if (GUILayout.Button("Replace All"))
        {
            foreach (GameObject selectedObject in Selection.objects)
            {
                ReplaceButton(selectedObject);
            }
        }

        foreach (GameObject selectedObject in Selection.objects)
        {
            // Skip if the selected object is not a GameObject
            if (selectedObject == null || selectedObject.GetComponent<Button>() == null)
                continue;

            GUILayout.Label(selectedObject.name);
            
        }
    }

    private void ReplaceButton(GameObject buttonObject)
    {
      
        PressableButton pressableButton = buttonObject.AddComponent<PressableButton>();

        pressableButton.OnClicked = buttonObject.GetComponent<Button>().onClick;

        DestroyImmediate(buttonObject.GetComponent<Button>());

        buttonObject.AddComponent<UGUIInputAdapter>();

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();

        Vector3 size = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 25f);

        BoxCollider collider = buttonObject.AddComponent<BoxCollider>();

        collider.center = buttonObject.GetComponent<RectTransform>().anchoredPosition;
        collider.size = size;



    }

    private void DebugOnClickEvents(Button button)
    {
        if (button == null)
        {
            Debug.LogWarning("No button selected.");
            return;
        }

        // Clear previous logs
        Debug.ClearDeveloperConsole();

        int call = button.onClick.GetPersistentEventCount();

        for (int i = 0; i < call; i++)
        {
            var target = button.onClick.GetPersistentTarget(i);
            var methodName = button.onClick.GetPersistentMethodName(i);

            Debug.Log($"Button OnClick event at index {i}:");
            Debug.Log($"Target: {target}");
            Debug.Log($"Method Name: {methodName}");
        }
    }
}
