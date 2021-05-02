using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEvent.Component;



namespace BasicEvent
{
    public enum EventType
    {
        OnCollisionEnter2D,
        FixedUpdate,
        OnCollisionExit2D
    }

    public static class EventAction
    {
        private static Type GetType(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.OnCollisionEnter2D:
                    return typeof(UnityEvent_OnCollisionEnter2D);
                case EventType.OnCollisionExit2D:
                    return typeof(UnityEvent_OnCollisionExit2D);
                case EventType.FixedUpdate:
                    return typeof(UnityEvent_FixedUpdate);
            }
            return null;
        }

        private static BaseClass GetOrCreateComponent(GameObject gameObject, EventType type)
        {
            BaseClass com;
            Type comType = GetType(type);
            UnityEngine.Component component = gameObject.GetComponent(comType);
            if (component == null)
            {
                component = gameObject.AddComponent(comType);
            }
            com = (BaseClass)component;

            return com;
        }
        private static BaseClass GetComponent(GameObject gameObject, EventType type)
        {
            BaseClass com;
            Type comType = GetType(type);
            UnityEngine.Component component = gameObject.GetComponent(comType);
            com = (BaseClass)component;

            return com;
        }
        public static void Add(GameObject gameObject, EventType type, Action action)
        {
            BaseClass com = GetOrCreateComponent(gameObject, type);
            com.AddAction(type, action);
        }
        public static void Add<T>(GameObject gameObject, EventType type, Action<T> action)
        {
            BaseClass com = GetOrCreateComponent(gameObject, type);
            com.AddAction<T>(type, action);
        }


        public static void Remove(GameObject gameObject, EventType type, Action action)
        {
            BaseClass com = GetComponent(gameObject, type);
            com.RemoveAction(action);
        }

    }


    namespace Component
    {
        public class BaseClass : MonoBehaviour
        {
            private Dictionary<EventType, Delegate> actionDic = new Dictionary<EventType, Delegate>();
            public void AddAction(EventType type, Action action)
            {
                bool hasKey = actionDic.ContainsKey(type);
                if (!hasKey)
                {
                    actionDic.Add(type, null);
                }
            }
            public void AddAction<T>(EventType type, Action<T> action)
            {
                bool hasKey = actionDic.ContainsKey(type);
                if (!hasKey)
                {
                    actionDic.Add(type, null);
                }
                Debug.Log(998);
                Debug.Log(typeof(T));
                if (typeof(T) == typeof(Collision2D))
                {
                    Debug.Log(999);
                    Action<Collision2D> ac = actionDic[type] as Action<Collision2D>;
                    ac += action as Action<Collision2D>;
                    actionDic[type] = ac;
                }

            }
            public void RemoveAction(Action action)
            {
                // this.action -= action;
            }





            public void Invoke<T>(EventType type, T data)
            {
                bool hasKey = actionDic.ContainsKey(type);
                if (hasKey)
                {
                    Delegate del = actionDic[type];
                    if (type == EventType.OnCollisionEnter2D)
                    {
                        Action<Collision2D> ac = del as Action<Collision2D>;
                        if (ac == null) { return; }
                        ac.Invoke(data as Collision2D);
                    }

                }
            }
            public void Invoke(EventType type)
            {
               
            }



        }
    }


}

