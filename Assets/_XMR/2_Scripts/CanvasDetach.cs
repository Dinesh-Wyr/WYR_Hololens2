using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDetach : MonoBehaviour
{
    [SerializeField]
    Transform newParent;

    private void Start()
    {
        StartCoroutine(delay());
        
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent = newParent;
    }
}
