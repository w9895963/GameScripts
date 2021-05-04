using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public static class ObjectState
{
    public static class State
    {

        public static void Add(GameObject gameObject, StateName state)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            List<StateName> states = com.states;
            if (states.Contains(state))
            {
                return;
            }
            states.Add(state);


            Dictionary<StateName, Action> dic = com.onStateAdd;
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
        public static void Remove(GameObject gameObject, StateName state)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            if (com == null) { return; }
            List<StateName> states = com.states;
            if (!states.Contains(state))
            {
                return;
            }
            states.Remove(state);


            Dictionary<StateName, Action> dic = com.onStateRemove;
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
        public static void Add(GameObject gameObject, StateName state, Action action)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            Dictionary<StateName, Action> dic = com.onStateAdd;
            bool hasKey = dic.ContainsKey(state);
            if (!hasKey)
            {
                dic.Add(state, action);
            }
        }
        public static void Remove(GameObject gameObject, StateName state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            Dictionary<StateName, Action> dic = com.onStateAdd;
            bool hasKey = dic.ContainsKey(state);
            if (hasKey)
            {
                dic[state] -= action;
            }
        }
    }
    public static class OnStateRemove
    {
        public static void Add(GameObject gameObject, StateName state, Action action)
        {
            ObjectStateComponent com = gameObject.GetOrAddComponent<ObjectStateComponent>();
            Dictionary<StateName, Action> dic = com.onStateRemove;
            bool hasKey = dic.ContainsKey(state);
            if (!hasKey)
            {
                dic.Add(state, action);
            }
        }
        public static void Remove(GameObject gameObject, StateName state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            Dictionary<StateName, Action> dic = com.onStateRemove;
            bool hasKey = dic.ContainsKey(state);
            if (hasKey)
            {
                dic[state] -= action;
            }
        }
    }
}