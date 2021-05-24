using System;
using System.Collections;
using System.Collections.Generic;
using DateBundle;
using UnityEngine;

namespace DateBundle
{
    public static class GlobalDate
    {
        public static Dictionary<GameObject, DateHolder> ObjectDict = new Dictionary<GameObject, DateHolder>();


    }
    public class DateHolder
    {
        public Dictionary<System.Type, System.Object> dateDict = new Dictionary<System.Type, System.Object>();
        public Dictionary<System.Type, Delegate> onDateUpdateDict = new Dictionary<System.Type, Delegate>();


        public static void AddDate<T, D>(GameObject gameObject, D date)
        {
            DateHolder objectDate = GlobalDate.ObjectDict.GetOrCreate(gameObject);
            Type key = typeof(T);
            objectDate.dateDict.Set(key, date);
            var action = objectDate.onDateUpdateDict.TryGetValue(key) as Action<D>;
            action?.Invoke(date);
        }

        public static D GetDate<T, D>(GameObject gameObject)
        {
            DateHolder objectDate = GlobalDate.ObjectDict[gameObject];
            Type key = typeof(T);

            return (D)objectDate.dateDict[key];

        }
        public static void AddAction<T, D>(GameObject gameObject, Action<D> action)
        {
            DateHolder objectDate = GlobalDate.ObjectDict.GetOrCreate(gameObject);
            var acD = objectDate.onDateUpdateDict;
            System.Type key = typeof(T);

            Action<D> act = acD.GetOrCreate(key, null) as Action<D>;
            act += action;
            acD[key] = act as Delegate;
        }
    }

}

public static class DateF
{
    public static void AddDate<T, D>(GameObject gameObject, D date)
    {
        DateHolder.AddDate<T, D>(gameObject, date);
    }

    public static D GetDate<T, D>(GameObject gameObject)
    {
        return DateHolder.GetDate<T, D>(gameObject);

    }
}
