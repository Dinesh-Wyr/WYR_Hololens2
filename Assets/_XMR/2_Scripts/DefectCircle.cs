using System.Collections.Generic;
using UnityEngine;

public class DefectCircle : MonoBehaviour
{

    [SerializeField]
    GameObject defectCircle;

    List<GameObject> droppedCircles = new List<GameObject>();

    public static DefectCircle Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void MarkDefect()
    {
        if (PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.Defect)
            return;

        GameObject g = Instantiate(defectCircle);
        g.transform.position = HandTrackingManager.GetRightIndexTip().position;

        GameObject circle = g.transform.GetChild(0).GetChild(0).gameObject;

        //Debug.Log(circle.name);

        circle.transform.rotation = Quaternion.LookRotation(circle.transform.position - HandTrackingManager.GetRightIndexTip().position);
        circle.transform.forward = HandTrackingManager.GetRightIndexTip().right;

        droppedCircles.Add(circle);
    }

    

    public void ClearDefect()
    {
        foreach(GameObject g in droppedCircles)
        {
            Destroy(g);
        }

        droppedCircles = new List<GameObject>();
    }

    public void RemoveDefect()
    {
        if (droppedCircles != null && droppedCircles.Count > 0)
        {
            Destroy(droppedCircles[droppedCircles.Count-1]);
            droppedCircles.RemoveAt(droppedCircles.Count-1);
        }
    }


    private void OnEnable()
    {
        droppedCircles = new List<GameObject>();
    }

}
