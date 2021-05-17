using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public static class ObjectState
{
    public static class State
    {


        public static void Add(GameObject gameObject, System.Enum state)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            var states = com.statesE;
            if (states.Contains(state))
            {
                return;
            }
            states.Add(state);


            var dic = com.onStateAddE;
            if (dic.ContainsKey(state))
            {
                Action action = dic[state];
                action?.Invoke();

            }
            Action onStateChanged = com.onStateChanged;
            if (onStateChanged != null)
            {
                onStateChanged();
            }

        }

        public static void Remove(GameObject gameObject, System.Enum state)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            if (com == null) { return; }
            var states = com.statesE;
            if (!states.Contains(state))
            {
                return;
            }
            states.Remove(state);


            var dic = com.onStateRemoveE;
            if (dic.ContainsKey(state))
            {
                Action action = dic[state];
                if (action != null)
                {
                    action();
                }

            }
            Action onStateChanged = com.onStateChanged;
            if (onStateChanged != null)
            {
                onStateChanged();
            }


        }




    }
    public static class OnStateAdd
    {

        public static void Add(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            var dic = com.onStateAddE;
            bool hasKey = dic.ContainsKey(state);
            if (!hasKey)
            {
                dic.Add(state, action);
            }
            else
            {
                dic[state] += action;
            }
        }
        public static void Remove(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            var dic = com.onStateAddE;
            bool hasKey = dic.ContainsKey(state);
            if (hasKey)
            {
                dic[state] -= action;
            }
        }
    }
    public static class OnStateRemove
    {

        public static void Add(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            var dic = com.onStateRemoveE;
            bool hasKey = dic.ContainsKey(state);
            if (!hasKey)
            {
                dic.Add(state, action);
            }
            else
            {
                dic[state] += action;
            }
        }
        public static void Remove(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            var dic = com.onStateRemoveE;
            bool hasKey = dic.ContainsKey(state);
            if (hasKey)
            {
                dic[state] -= action;
            }
        }
    }
    public static class OnStateChanged
    {
        public static void Add(GameObject gameObject, Action action)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            com.onStateChanged += action;

        }
        public static void Remove(GameObject gameObject, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            com.onStateChanged -= action;
        }
    }
}