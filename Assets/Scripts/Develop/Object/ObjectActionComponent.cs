using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectActionComponent : MonoBehaviour
{
    public Dictionary<ObjectActionName, Action<System.Object>> actions = new Dictionary<ObjectActionName, Action<object>>();
}
