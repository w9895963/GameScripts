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
        public Dictionary<System.Type, Delegate> dateGetDict = new Dictionary<System.Type, Delegate>();
        public Dictionary<System.Type, Delegate> onDateUpdateDict = new Dictionary<System.Type, Delegate>();



        public static void SetDate<T, D>(System.Object obj, Func<D> getFunc)
        {
            Dictionary<object, DateHolder> dic = GlobalDate.DateHolderDict;
            DateHolder objectDate = dic.TryGetValue(obj);
            if (objectDate == null)
            {
                dic.Add(obj, new DateHolder());
                objectDate = dic[obj];
            }

            Type key = typeof(T);
            objectDate.dateGetDict.Set(key, getFunc);
        }






        public static void AddDate<T, D>(System.Object obj, D date)
        {
            SetDate<T, D>(obj, () => date);
            CallAction<T, D>(obj);
        }

        public static D GetDate<T, D>(System.Object obj)
        {
            DateHolder objectDate = GlobalDate.DateHolderDict[obj];
            Type key = typeof(T);

            return ((Func<D>)objectDate.dateGetDict[key]).Invoke();

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

        public static void CallAction<T, D>(System.Object obj)
        {
            Dictionary<object, DateHolder> dic = GlobalDate.DateHolderDict;
            DateHolder objectDate = dic.TryGetValue(obj);
            Type key = typeof(T);
            var date = ((Func<D>)objectDate.dateGetDict[key]).Invoke();
            var action = objectDate.onDateUpdateDict.TryGetValue(key) as Action<D>;
            action?.Invoke(date);
        }



        public static O FindObjectOfDate<O, T, D>(D date) where O : class
        {

            foreach (System.Object DateHolderKey in GlobalDate.DateHolderDict.Keys)
            {
                if (DateHolderKey == null)
                {
                    GlobalDate.DateHolderDict.Remove(DateHolderKey);
                }


                Dictionary<Type, Delegate> dateFuncDict = GlobalDate.DateHolderDict[DateHolderKey].dateGetDict;
                Type key = typeof(T);
                bool v = dateFuncDict.ContainsKey(key);
                if (!v) continue;

                System.Object d = date;
                System.Object d1 = ((Func<D>)dateFuncDict[key]).Invoke();
                if (d == d1) return (O)DateHolderKey;
            }
            return null;

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
    public static void AddAction<T>(System.Object obj, Action<T> action)
    {
        DateBundle.DateHolder.AddAction<T, T>(obj, action);
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



