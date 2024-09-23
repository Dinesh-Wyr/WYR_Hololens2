using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivatedColor : MonoBehaviour
{
    public Color activateColor;
    public Color deactivateColor;

    public Image[] butttonsImage;
    
    public void ChangeColor(int index)
    {
        butttonsImage[index].color = activateColor;

        for (int i = 0; i < butttonsImage.Length; i++)
        {
            if(i != index)
                butttonsImage[i].color = deactivateColor;
        }
    }
}
