using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour
{
    [SerializeField]
    Toggle toggleComponent;
    [SerializeField]
    GameObject radioButtonInner;
    // Start is called before the first frame update
    void Start()
    {
        radioButtonInner.SetActive(toggleComponent.isOn);
    }

    public void ToggleRadioImage()
    {
        radioButtonInner.SetActive(toggleComponent.isOn);
    }
}
