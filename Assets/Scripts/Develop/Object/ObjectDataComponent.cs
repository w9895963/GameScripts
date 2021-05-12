using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ObjectDataComponent : MonoBehaviour
{
    public List<string> data = new List<string>();

    public Dictionary<ObjectDataName, Action<System.Object>> onDataUpdate = new Dictionary<ObjectDataName, Action<System.Object>>();
}

