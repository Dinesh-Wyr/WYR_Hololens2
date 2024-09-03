using UnityEngine;

public class MeasurementLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    GameObject measurementTextObj;
    [SerializeField]
    TMPro.TMP_Text measurementText;

    public void SetMeasurementInCM()
    {
        measurementTextObj.SetActive(true);
        measurementTextObj.transform.rotation = Quaternion.LookRotation(measurementTextObj.transform.position - Camera.main.transform.position);

        Vector3 p1 = lineRenderer.GetPosition(0);
        Vector3 p2 = lineRenderer.GetPosition(1);

        measurementText.text = (Vector3.Distance(p1, p2) * 100).ToString("F2") + " cm";

        Vector3 middlePoint = (p1 + p2) / 2;

        // set display position of labels 
        measurementTextObj.transform.position = middlePoint + Vector3.up * 0.01f;
        measurementTextObj.transform.forward = Camera.main.transform.forward;
    }

    public void SetMeasurementInInch()
    {
        measurementTextObj.SetActive(true);
        measurementTextObj.transform.rotation = Quaternion.LookRotation(measurementTextObj.transform.position - Camera.main.transform.position);

        Vector3 p1 = lineRenderer.GetPosition(0);
        Vector3 p2 = lineRenderer.GetPosition(1);

        measurementText.text = (Vector3.Distance(p1, p2) * 39.3701f).ToString("F2") + " in";

        Vector3 middlePoint = (p1 + p2) / 2;

        // set display position of labels 
        measurementTextObj.transform.position = middlePoint + Vector3.up * 0.01f;
        measurementTextObj.transform.forward = Camera.main.transform.forward;
    }

    private void OnEnable()
    {
        UIEventSystem.OnMeasureInCm += SetMeasurementInCM;
        UIEventSystem.OnMeasureInInch += SetMeasurementInInch;
    }

    private void OnDisable()
    {
        UIEventSystem.OnMeasureInCm -= SetMeasurementInCM;
        UIEventSystem.OnMeasureInInch -= SetMeasurementInInch;
    }
}