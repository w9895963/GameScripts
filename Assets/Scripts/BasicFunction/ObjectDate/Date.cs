using System;
using System.Collections;
using System.Collections.Generic;
using DateBundle;
using UnityEngine;

namespace DateBundle
{
    public static class GlobalDate
    {
        public static Dictionary<System.Object, DateHolder> ObjectDict = new Dictionary<System.Object, DateHolder>();


    }
    public class DateHolder
    {
        public Dictionary<System.Type, System.Object> dateDict = new Dictionary<System.Type, System.Object>();
        public Dictionary<System.Type, Delegate> onDateUpdateDict = new Dictionary<System.Type, Delegate>();



        public static void AddDate<T, D>(System.Object obj, D date)
        {
            DateHolder objectDate = GlobalDate.ObjectDict.GetOrCreate(obj);
            Type key = typeof(T);
            objectDate.dateDict.Set(key, date);
            var action = objectDate.onDateUpdateDict.TryGetValue(key) as Action<D>;
            action?.Invoke(date);
        }


        public static D GetDate<T, D>(System.Object obj)
        {
            DateHolder objectDate = GlobalDate.ObjectDict[obj];
            Type key = typeof(T);

            return (D)objectDate.dateDict[key];

        }

        public static void AddAction<T, D>(System.Object obj, Action<D> action)
        {
            DateHolder objectDate = GlobalDate.ObjectDict.GetOrCreate(obj);
            var acD = objectDate.onDateUpdateDict;
            System.Type key = typeof(T);

            Action<D> act = acD.GetOrCreate(key, null) as Action<D>;
            act += action;
            acD[key] = act as Delegate;
        }
        public static void RemoveAction<T, D>(System.Object obj, Action<D> action)
        {
            DateHolder objectDate = GlobalDate.ObjectDict.GetOrCreate(obj);
            var acD = objectDate.onDateUpdateDict;
            System.Type key = typeof(T);

            Action<D> act = acD.GetOrCreate(key, null) as Action<D>;
            act -= action;
            acD[key] = act as Delegate;
        }
    }

}

public static class DateF
{
    public static void AddDate<T, D>(System.Object obj, D date)
    {
        DateHolder.AddDate<T, D>(obj, date);
    }
    public static void AddDate<T>(System.Object obj, T date)
    {
        DateHolder.AddDate<T, T>(obj, date);
    }





    public static D GetDate<T, D>(System.Object obj)
    {
        return DateHolder.GetDate<T, D>(obj);

    }
    public static T GetDate<T>(System.Object obj)
    {
        return DateHolder.GetDate<T, T>(obj);

    }



    public static void AddAction<T, D>(System.Object obj, Action<D> action)
    {
        DateBundle.DateHolder.AddAction<T, D>(obj, action);
    }

}



