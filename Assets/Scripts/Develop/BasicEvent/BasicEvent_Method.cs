using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEvent
{
    public static class Method
    {
        public static void Get<T>(GameObject gameObject, Action<T> action) where T : MonoBehaviour
        {
            T com = gameObject.GetComponent<T>();
            if (com == null) { return; }
            action(com);
        }
        public static void GetOrCreate<T>(GameObject gameObject, Action<T> action) where T : MonoBehaviour
        {
            T com = gameObject.GetComponent<T>();
            if (com == null)
            {
                com = gameObject.AddComponent<T>();
            }
            action(com);
        }
       
        public static void RemoveCommon<T>(GameObject gameObject, Func<T, Delegate> RemoveAction) where T : MonoBehaviour
        {
            BasicEvent.Method.GetOrCreate<T>(gameObject, (com) =>
            {
                Delegate d = RemoveAction(com);
                if (d == null) { com.Destroy(); }
            });
        }



    }
}

