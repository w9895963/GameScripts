using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;





public static class ObjectState
{
    public static class State
    {


        public static void Add(GameObject gameObject, System.Enum state)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>(true);
            var states = com.states;
            if (states.Contains(state))
            {
                return;
            }
            states.Add(state);


            var dic = com.onStateAdd;
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
            var states = com.states;
            if (!states.Contains(state))
            {
                return;
            }
            states.Remove(state);


            var dic = com.onStateRemove;
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

        public static void Remove(GameObject gameObject, params System.Enum[] state)
        {
            state.ForEach((x) => Remove(gameObject, x));
        }


        public static bool HasAny(GameObject gameObject, params System.Enum[] statesin)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            if (com == null) { return false; }
            var allStates = com.states;

            int v = statesin.Except(allStates).Count();

            return v < statesin.Count();
        }



    }
    public static class OnStateAdd
    {

        public static void Add(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>(true);
            var dic = com.onStateAdd;
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
        public static void Add(GameObject gameObject, Action action, params System.Enum[] states)
        {
            states.ForEach((st) => Add(gameObject, st, action));
        }
        public static void Remove(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            var dic = com.onStateAdd;
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
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>(true);
            var dic = com.onStateRemove;
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
        public static void Add(GameObject gameObject, Action action, params System.Enum[] states)
        {
            states.ForEach((state) =>
            {
                Add(gameObject, state, action);
            });
        }


        public static void Remove(GameObject gameObject, System.Enum state, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            var dic = com.onStateRemove;
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
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>(true);
            com.onStateChanged += action;

        }
        public static void Remove(GameObject gameObject, Action action)
        {
            ObjectStateComponent com = gameObject.GetComponent<ObjectStateComponent>();
            com.onStateChanged -= action;
        }
    }
}