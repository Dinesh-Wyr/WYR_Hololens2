using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPinsScript : MonoBehaviour
{
    public static DropPinsScript Instance;

    
    [SerializeField]
    GameObject dropPinPrefab;

    Vector3 fingerTipPosition;

    public List<GameObject> droppedPins = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
    }


    IEnumerator Start()
    {
        // Infinite loop that checks if user is pinching
        while (true)
        {
            // Draws a new line every 2 secs
            yield return new WaitForSeconds(2);

            // check if both hands are tracked
            if (PalmWheelManager.Instance.SelectedTab == PalmWheelTabs.MultiPointGesture)
            {
                if (HandTrackingManager.IsLeftMiddleFingerPinching())
                {
                    fingerTipPosition = HandTrackingManager.GetLeftMiddleTip().position;
                    DropPin();
                }

                if (HandTrackingManager.IsRightMiddleFingerPinching())
                {
                    fingerTipPosition = HandTrackingManager.GetRightMiddleTip().position;
                    DropPin();
                }
            }

        }
    }

    public void MultiPointVoice()
    {
        if (PalmWheelManager.Instance.SelectedTab != PalmWheelTabs.MultiPointVoice)
            return;

        fingerTipPosition = HandTrackingManager.GetRightIndexTip().position;
        DropPin();
    }

    void DropPin()
    {
        GameObject g = Instantiate(dropPinPrefab);
        g.transform.position = fingerTipPosition;

        GameObject pin = g.transform.GetChild(0).GetChild(0).gameObject;

        pin.transform.rotation = Quaternion.LookRotation(pin.transform.position - Camera.main.transform.position);
        //pin.transform.forward = rightIndexTip.right;

        droppedPins.Add(pin);
    }



    public void ClearAllPins()
    {
        foreach (GameObject g in droppedPins)
        {
            Destroy(g);
        }
        droppedPins = new List<GameObject>();
    }

    public void RemoveLastPin()
    {
        if (droppedPins != null && droppedPins.Count > 0)
        {
            Destroy(droppedPins[droppedPins.Count - 1]);
            droppedPins.RemoveAt(droppedPins.Count - 1);
        }
    }



    private void OnEnable()
    {
        droppedPins = new List<GameObject>();
    }
}
