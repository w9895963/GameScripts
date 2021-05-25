using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ObjectDateComponent : MonoBehaviour
{
   

    public Dictionary<System.Enum, System.Object> dateDict = new Dictionary<Enum, object>();
    public Dictionary<System.Enum, Action<System.Object>> onDateUpdate = new Dictionary<System.Enum, Action<System.Object>>();

}

