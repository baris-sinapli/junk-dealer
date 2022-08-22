using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBrush : MonoBehaviour
{
    private bool isBrushActive = true;

    public bool IsBrushActive { get => isBrushActive; set => isBrushActive = value; }

    public void BrushModeActivated()
    {
        IsBrushActive = true;
    }

    public void HandModeActivated()
    {
        IsBrushActive = false;
    }
}
