using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RequiredDocsRow
{
    public bool client;
    public bool vendor;
    public bool not;
    public string remarks;
}

public class RequiredDocsItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI label;
    [SerializeField]
    Toggle client;
    [SerializeField]
    Toggle vendor;
    [SerializeField]
    Toggle notAvailable;
    [SerializeField]
    TMP_InputField remarks;

    RequiredDocsGetField field = new RequiredDocsGetField();

    public RequiredDocsGetField GetData()
    {
        field.receivedClient = client.isOn;
        field.receivedVendor = vendor.isOn;
        field.NA = notAvailable.isOn;
        field.remarks = remarks.text;

        return field;
    }

    public void PopulateRequiredDocsItem(RequiredDocsGetField field)
    {
        this.field = field;

        label.text = field.document;
        client.isOn = field.receivedClient;
        vendor.isOn = field.receivedVendor;
        notAvailable.isOn = field.NA;
        remarks.text = field.remarks;
    }

    public void SelectInputField()
    {
        KeyboardManager.Instance.isSaveComments = false;
        KeyboardManager.Instance.ShowVoiceEnabledKeyboard(remarks);
    }
}
