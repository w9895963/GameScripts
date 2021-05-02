using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BasicEvent
{
    public class UnityEventBaseType : MonoBehaviour
    {
        public ActionClass action = new ActionClass();
        public class ActionClass
        {
            public System.Action actionEmpty;
            public Action<Collision2D> actionCollision2d;
        }


        public void Invoke(Collision2D collision)
        {
           
            Action<Collision2D> action = this.action.actionCollision2d;
            if (action != null)
            { return; }
            action(collision);
        }
        public void Invoke()
        {
            System.Action action = this.action.actionEmpty;
            if (action != null)
            { return; }
            action();
        }

        public static void Add<T>(GameObject gameObject, Action<Collision2D> action) where T : UnityEventBaseType
        {
            T t = gameObject.GetComponent<T>();
            if (t == null)
            {
                t = gameObject.AddComponent<T>();
            }
            t.action.actionCollision2d += action;

        }
        public static void Add<T>(GameObject gameObject, System.Action action) where T : UnityEventBaseType
        {
            T t = gameObject.GetComponent<T>();
            if (t == null)
            {
                t = gameObject.AddComponent<T>();
            }
            t.action.actionEmpty += action;

        }

        public static void Remove<T>(GameObject gameObject, Action<Collision2D> action) where T : UnityEventBaseType
        {
            T t = gameObject.GetComponent<T>();
            if (t != null)
            {
                t.action.actionCollision2d -= action;
            }
        }
        public static void Remove<T>(GameObject gameObject, System.Action action) where T : UnityEventBaseType
        {
            T t = gameObject.GetComponent<T>();
            if (t != null)
            {
                t.action.actionEmpty -= action;
            }
        }


    }
}


