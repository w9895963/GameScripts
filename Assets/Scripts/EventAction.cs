using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;

public class EventAction : MonoBehaviour {
    public void _CUrsor_VisibleSystemCursor (bool enabled) => CursorCtrl.ShowSystemCursor (enabled);
}