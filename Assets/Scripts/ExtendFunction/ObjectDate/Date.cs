using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DateBundle;
using UnityEngine;

namespace DateBundle
{
    public static class GlobalDate
    {
        public static Dictionary<System.Object, DateHolder> DateHolderDict = new Dictionary<System.Object, DateHolder>();


    }
    public class DateHolder
    {
        public Dictionary<System.Type, System.Object> dateDict = new Dictionary<System.Type, System.Object>();
        public Dictionary<System.Type, Delegate> onDateUpdateDict = new Dictionary<System.Type, Delegate>();



        public static void AddDate<T, D>(System.Object obj, D date)
        {
            Dictionary<object, DateHolder> dic = GlobalDate.DateHolderDict;
            DateHolder objectDate = dic.TryGetValue(obj);
            if (objectDate == null)
            {
                dic.Add(obj, new DateHolder());
                objectDate = dic[obj];
            }

            Type key = typeof(T);
            objectDate.dateDict.Set(key, date);
            var action = objectDate.onDateUpdateDict.TryGetValue(key) as Action<D>;
            action?.Invoke(date);
        }



        public static D GetDate<T, D>(System.Object obj)
        {
            DateHolder objectDate = GlobalDate.DateHolderDict[obj];
            Type key = typeof(T);

            return (D)objectDate.dateDict[key];

        }

        public static void AddAction<T, D>(System.Object obj, Action<D> action)
        {
            DateHolder objectDate = GlobalDate.DateHolderDict.GetOrCreate(obj);
            var acD = objectDate.onDateUpdateDict;
            System.Type key = typeof(T);

            Action<D> act = acD.GetOrCreate(key, null) as Action<D>;
            act += action;
            acD[key] = act as Delegate;
        }
        public static void RemoveAction<T, D>(System.Object obj, Action<D> action)
        {
            DateHolder objectDate = GlobalDate.DateHolderDict.GetOrCreate(obj);
            var acD = objectDate.onDateUpdateDict;
            System.Type key = typeof(T);

            Action<D> act = acD.GetOrCreate(key, null) as Action<D>;
            act -= action;
            acD[key] = act as Delegate;
        }



        public static O FindObjectOfDate<O, T, D>(D date) where O : class
        {

            foreach (System.Object DateHolderKey in GlobalDate.DateHolderDict.Keys)
            {
                if (DateHolderKey == null)
                {
                    GlobalDate.DateHolderDict.Remove(DateHolderKey);
                }


                Dictionary<Type, System.Object> dateDic = GlobalDate.DateHolderDict[DateHolderKey].dateDict;
                Type key = typeof(T);
                bool v = dateDic.ContainsKey(key);
                if (!dateDic.ContainsKey(key)) continue;

                System.Object d = date;
                if (d == dateDic[key]) return (O)DateHolderKey;
            }
            return null;

            // KeyValuePair<object, DateHolder>[] keyValuePairs = GlobalDate.DateHolderDict.ToArray();

            // KeyValuePair<object, DateHolder> keyValuePair = keyValuePairs.ToList().Find((p) =>
            // {
            //     if (p.Key.GetType() == typeof(GameObject))
            //     {
            //         if (((GameObject)p.Key) == null)
            //         {
            //             GlobalDate.DateHolderDict.Remove(p.Key);
            //             return false;
            //         }
            //     }
            //     Dictionary<Type, object> dateDic = p.Value.dateDict;
            //     Type key = typeof(T);
            //     bool v = dateDic.ContainsKey(key);
            //     if (!v) return false;
            //     System.Object d = date;
            //     if (d == dateDic[key]) return true;
            //     return false;
            // });

            // if (keyValuePair.Key == null) { return null; }

            // return (O)keyValuePair.Key;
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



    public static O FindObjectOfDate<O, T, D>(D date) where O : class
    {
        return DateBundle.DateHolder.FindObjectOfDate<O, T, D>(date);
    }
    public static O FindObjectOfDate<O, D>(D date) where O : class
    {
        return DateBundle.DateHolder.FindObjectOfDate<O, D, D>(date);
    }

}



