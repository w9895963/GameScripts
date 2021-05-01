using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEventAction : MonoBehaviour
{

    public void ToggleIndicator() => Indicator.ToggleIndicator();
    public void ToggleIndicator(bool enabled) => Indicator.ToggleIndicator(enabled);
    public bool Tog { set; get; }

}
