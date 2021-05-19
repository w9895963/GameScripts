using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;











public class ObjectStateComponent : MonoBehaviour
{

    public Action onStateChanged;
    public List<System.Enum> states = new List<System.Enum>();
    public Dictionary<System.Enum, Action> onStateAdd = new Dictionary<System.Enum, Action>();
    public Dictionary<System.Enum, Action> onStateRemove = new Dictionary<System.Enum, Action>();
}
