using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;











public class ObjectStateComponent : MonoBehaviour
{
    public List<ObjectStateName> states = new List<ObjectStateName>();
    public Dictionary<ObjectStateName, Action> onStateAdd = new Dictionary<ObjectStateName, Action>();
    public Dictionary<ObjectStateName, Action> onStateRemove = new Dictionary<ObjectStateName, Action>();
    public Action onStateChanged;
    public List<System.Enum> statesE = new List<System.Enum>();
    public Dictionary<System.Enum, Action> onStateAddE = new Dictionary<System.Enum, Action>();
    public Dictionary<System.Enum, Action> onStateRemoveE = new Dictionary<System.Enum, Action>();
}
