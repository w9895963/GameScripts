using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static State.GlobalDate;

namespace State
{




    public static class GlobalDate
    {
        public static Dictionary<GameObject, StateDate> stateDateDic = new Dictionary<GameObject, StateDate>();

    }

    public class StateDate
    {
        public Dictionary<System.Type, State> StateDic = new Dictionary<Type, State>();
        public Dictionary<System.Type, List<State>> CheckStateList = new Dictionary<Type, List<State>>();
        public List<State> AllStates => StateDic.Values.ToList();
        public List<State> AllEnableStates => StateDic.Values.Where((st) => st.Enabled).ToList();

    }

    public class State
    {
        public GameObject gameObject;
        public List<System.Type> activeCondition_Exist = new List<Type>();
        public List<System.Type> activeCondition_Except = new List<Type>();
        public Action OnEnable;
        public Action OnDisable;
        bool enabled;
        public bool Enabled => enabled;

        List<State> AllStates => stateDateDic[gameObject].AllStates;
        List<State> CheckStateList => stateDateDic[gameObject].CheckStateList.TryGetValue(this.GetType());
        List<System.Type> AllActiveStates => stateDateDic[gameObject].AllEnableStates.Select((x) => x.GetType()).ToList();






        private void UpdateStates(List<State> states)
        {
            states?.ForEach((st) =>
            {
                bool checkPass = true;
                List<Type> exist = st.activeCondition_Exist;
                List<Type> allActiveStates = AllActiveStates;
                if (exist.Except(allActiveStates).Count() > 0)
                {
                    checkPass = false;
                }

                List<Type> except = st.activeCondition_Except;
                if (except.Except(allActiveStates).Count() < except.Count)
                {
                    checkPass = false;
                }
                if (checkPass)
                {
                    st.Enable();
                }
                else
                {
                    st.Disable();
                }

            });
        }


        private void Enable()
        {
            if (enabled == true)
            {
                return;
            }

            enabled = true;
            OnEnable?.Invoke();
            UpdateStates(CheckStateList);

        }
        private void Disable()
        {
            if (enabled == false)
            {
                return;
            }

            enabled = false;
            OnDisable?.Invoke();
            UpdateStates(CheckStateList);
        }


        private void OnObjectDestroyAction()
        {
            GlobalDate.stateDateDic.Remove(gameObject);
        }


        public static State GetState<T>(GameObject gameObject) where T : State, new()
        {
            StateDate stateDate = GlobalDate.stateDateDic.GetOrCreate(gameObject);
            State state = stateDate.StateDic.GetOrCreate(typeof(T), new T());
            state.gameObject = gameObject;

            BasicEvent.OnDestroyEvent.Add(gameObject, state.OnObjectDestroyAction);

            return state;
        }

        public static void SetState<T>(GameObject gameObject, bool enabled) where T : State, new()
        {
            State state = GetState<T>(gameObject);
            if (enabled)
            {
                state.Enable();
            }
            else
            {
                state.Disable();
            }

        }
        public static void SetStateCondition<T>(GameObject gameObject, List<System.Type> exist, List<System.Type> except) where T : State, new()
        {
            State state = GetState<T>(gameObject);
            state.activeCondition_Exist = exist;
            state.activeCondition_Except = except;

            Dictionary<Type, List<State>> checkList = stateDateDic[gameObject].CheckStateList;
            exist.ForEach((stateType) =>
            {
                List<State> states = checkList.GetOrCreate(stateType);
                states.AddNotHas(state);
            });


            Dictionary<Type, List<State>> onDisableCheck = stateDateDic[gameObject].CheckStateList;
            except.ForEach((stateType) =>
            {
                List<State> states = onDisableCheck.GetOrCreate(stateType);
                states.AddNotHas(state);
            });
        }

        public static void SetStateAction<T>(GameObject gameObject, Action onEnable = null, Action onDisable = null) where T : State, new()
        {
            State state = GetState<T>(gameObject);
            state.OnEnable += onEnable;
            state.OnDisable += onDisable;
        }



    }



    public class Fall : State
    {

    }
    public class OnGround : State
    {

    }



}

public static class StateF
{
    public static void AddStateCondition<T>(GameObject gameObject, List<System.Type> exist, List<System.Type> except) where T : State.State, new()
    {
        State.State.SetStateCondition<T>(gameObject, exist, except);
    }

    public static void SetState<T>(GameObject gameObject, bool enabled) where T : State.State, new()
    {
        State.State.SetState<T>(gameObject, enabled);

    }


    public static void AddStateAction<T>(GameObject gameObject, Action onEnable = null, Action onDisable = null) where T : State.State, new()
    {
        State.State.SetStateAction<T>(gameObject, onEnable, onDisable);
    }
}
