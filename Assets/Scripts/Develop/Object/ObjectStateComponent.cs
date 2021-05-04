using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;











public class ObjectStateComponent : MonoBehaviour
{
    public List<StateName> states = new List<StateName>();
    public Dictionary<StateName, Action> onStateAdd = new Dictionary<StateName, Action>();
    public Dictionary<StateName, Action> onStateRemove = new Dictionary<StateName, Action>();
    public Action onStateChanged;
}
