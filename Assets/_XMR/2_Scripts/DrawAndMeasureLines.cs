using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAndMeasureLines : MonoBehaviour
{
    public static DrawAndMeasureLines Instance;


    [SerializeField]
    GameObject lineRenderer;

    List<GameObject> multipointLines = new List<GameObject>();
    List<GameObject> twoPointsLines = new List<GameObject>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }


    private void OnEnable()
    {
        multipointLines = new List<GameObject>();
        twoPointsLines = new List<GameObject>();

        UIEventSystem.OnMeasureInCm += OnMeasureInCm;
    }

    private void OnDisable()
    {
        UIEventSystem.OnMeasureInCm -= OnMeasureInCm;
    }

    IEnumerator Start()
    {
        // Infinite loop that checks if user is pinching
        while (true)
        {
            // Draws a new line every 2 secs
            yield return new WaitForSeconds(2);

            // check if both hands are tracked
            if (HandTrackingManager.IsBothHandsTracked() && PalmWheelManager.Instance.SelectedTab == PalmWheelTabs.TwoPointGesture)
            {
                if (HandTrackingManager.IsBothHandsIndexPinching())
                {
                    TwoPointMeasurement();
                }
            }

        }
    }

    public void TwoPointVoice()
    {
        if (PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.TwoPointVoice)
            return;
        
        TwoPointMeasurement();

    }

    public void TwoPointMeasurement(Vector3? p1 = null, Vector3? p2 = null, bool multipoint = false)
    {
        Debug.Log("instantiating object");
        GameObject lr = Instantiate(this.lineRenderer);

        Debug.Log("drawing line");
        LineRenderer lineRenderer = lr.GetComponent<LineRenderer>();

        if (p1 == null || p2 == null)
        {
            lineRenderer.SetPosition(0, HandTrackingManager.GetLeftIndexTip().position);
            lineRenderer.SetPosition(1, HandTrackingManager.GetRightIndexTip().position);

            twoPointsLines.Add(lr);
        }

        if (p1 != null && p2 != null)
        {
            lineRenderer.SetPosition(0, (Vector3)p1);
            lineRenderer.SetPosition(1, (Vector3)p2);
        }

        if (multipoint)
        {
            multipointLines.Add(lr);
            //lineRenderer.enabled = false;
        }

        Debug.Log("line position : " + lineRenderer.transform.position);

        lineRenderer.GetComponent<MeasurementLine>().SetMeasurementInCM();
    }


    public void OnMeasureInCm()
    {
        ClearMultiPointsMeasurement();
        List<GameObject> droppedPins = DropPinsScript.Instance.droppedPins;
        multipointLines = new List<GameObject>();

        for (int i = 0; i < droppedPins.Count - 1; i++)
        {
            TwoPointMeasurement(droppedPins[i].transform.position, droppedPins[i + 1].transform.position, true);
        }

        TwoPointMeasurement(droppedPins[droppedPins.Count - 1].transform.position, droppedPins[0].transform.position, true);
    }


    public void ClearMultiPointsMeasurement()
    {

        Debug.Log("ClearMultiPointsMeasurement");

        foreach (GameObject g in multipointLines)
        {
            Destroy(g);
        }

    }

    public void ClearTwoPointsMeasurement()
    {
        Debug.Log("ClearTwoPointsMeasurement");
        foreach (GameObject g in twoPointsLines)
        {
            Destroy(g);
        }
    }

    public void RemoveLastMultiPointsLine()
    {

        Debug.Log("RemoveLastMultiPointsLine");

        if (PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.MultiPointGesture && PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.MultiPointVoice)
            return;

        if (multipointLines != null && multipointLines.Count > 0)
        {
            Destroy(multipointLines[multipointLines.Count - 1]);
            multipointLines.RemoveAt(multipointLines.Count - 1);
        }
    }

    public void RemoveLastTwoPointsLine()
    {
        Debug.Log("RemoveLastTwoPointsLine");

        if (PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.TwoPointGesture && PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.TwoPointVoice)
            return;

        if (twoPointsLines != null && twoPointsLines.Count > 0)
        {
            Destroy(twoPointsLines[twoPointsLines.Count - 1]);
            twoPointsLines.RemoveAt(twoPointsLines.Count - 1);
        }
    }

    public void MeasureInCm()
    {
        Debug.Log("MeasureInCm");

        TwoPointVoice();
        UIEventSystem.MeasureInCm();
    }

    public void MeasureInInch()
    {
        UIEventSystem.MeasureInInch();
    }

}
