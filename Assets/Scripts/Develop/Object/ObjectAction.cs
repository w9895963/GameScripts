using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectAction
{
    public static void AddAction(GameObject gameObject, ObjectActionName actionName, Action<System.Object> action)
    {
        ObjectActionComponent com = gameObject.GetOrAddComponent<ObjectActionComponent>();

        Action<object> ac = null;
        Dictionary<ObjectActionName, Action<object>> dic = com.actions;
        bool hasKey = dic.TryGetValue(actionName, out ac);
        if (!hasKey)
        {
            dic.Add(actionName, default);
        }


    }

}
