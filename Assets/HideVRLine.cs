using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideVRLine : MonoBehaviour
{
    public XRInteractorLineVisual vrLine;

    public void Hide()
    {
        vrLine.enabled = false;
    }

    public void Display()
    {
        vrLine.enabled = true;
    }
}
