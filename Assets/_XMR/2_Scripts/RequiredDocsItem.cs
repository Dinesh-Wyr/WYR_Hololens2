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
    Toggle client;
    [SerializeField]
    Toggle vendor;
    [SerializeField]
    Toggle notAvailable;
    [SerializeField]
    TMP_InputField remarks;

    public RequiredDocsRow GetData()
    {
        RequiredDocsRow row = new RequiredDocsRow();
        row.client = client.isOn;
        row.vendor = vendor.isOn;
        row.not = notAvailable.isOn;
        row.remarks = remarks.text;

        return row;
    }

    public void LoadData(RequiredDocsRow row)
    {
        client.isOn = row.client;
        vendor.isOn = row.vendor;
        notAvailable.isOn = row.not;
        remarks.text = row.remarks;
    }

    public void SelectInputField()
    {
        KeyboardManager.Instance.ShowVoiceEnabledKeyboard(remarks);
    }
}
