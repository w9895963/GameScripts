using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectDate
{
    public static void UpdateDate<T>(GameObject gameObject, ObjectDataName name, T data)
    {
        ObjectDataComponent com = gameObject.GetOrAddComponent<ObjectDataComponent>();
        var dic = com.onDataUpdate;
        if (!dic.ContainsKey(name))
        {
            dic.Add(name, null);
        }
        else
        {
            var action = dic[name];
            if (action != null) { action(data); }
        }
        ShowData();
        void ShowData()
        {
            int ind = new List<ObjectDataName>(dic.Keys).FindIndex((x) => x == name);
            List<string> dat = com.data;
            dat.Add(ind, name.ToString() + " : " + data.ToString());
        }
    }
  

 

    public static void AddUpdateAction(GameObject gameObject, ObjectDataName name, Action<System.Object> action)
    {
        ObjectDataComponent com = gameObject.GetOrAddComponent<ObjectDataComponent>();
        var dic = com.onDataUpdate;
        if (!dic.ContainsKey(name))
        {
            dic.Add(name, null);
        }

        dic[name] += action;
    }
}
