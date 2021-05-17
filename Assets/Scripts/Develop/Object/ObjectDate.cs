using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectDate
{




    public static void UpdateData(GameObject gameObject, System.Enum dateName, System.Object data)
    {
        ObjectDateComponent com = gameObject.GetOrAddComponent<ObjectDateComponent>();
        var dic = com.dateDict;
        if (dic.ContainsKey(dateName))
        {
            if (dic[dateName] != data)
            {
                dic[dateName] = data;
                Action<System.Object> action;
                com.onDateUpdate.TryGetValue(dateName, out action);
                action?.Invoke(data);
            }

        }
        else
        {
            dic.Add(dateName, data);
            Action<System.Object> action;
            com.onDateUpdate.TryGetValue(dateName, out action);
            action?.Invoke(data);
        }


    }
    public static void OnDateUpdate(GameObject gameObject, System.Enum dateName, Action<System.Object> action)
    {
        ObjectDateComponent com = gameObject.GetOrAddComponent<ObjectDateComponent>();
        var dic = com.onDateUpdate;
        if (!dic.ContainsKey(dateName))
        {
            dic.Add(dateName, null);
        }

        dic[dateName] += action;
    }
    public static bool TryGetData<T>(GameObject gameObject, System.Enum dateName, out T data)
    {
        ObjectDateComponent com = gameObject.GetOrAddComponent<ObjectDateComponent>();
        var dic = com.dateDict;
        if (dic.ContainsKey(dateName))
        {
            data = (T)dic[dateName];
            return true;

        }
        else
        {
            data = default;
            return false;
        }


    }
}
