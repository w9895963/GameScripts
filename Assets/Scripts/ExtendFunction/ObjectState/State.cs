using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static StateBundle.GlobalDate;

namespace StateBundle
{




    public static class GlobalDate
    {
        public static Dictionary<GameObject, StateDate> stateDateDic = new Dictionary<GameObject, StateDate>();

    }

    public class StateDate
    {
        public Dictionary<System.Type, StateInst> StateDic = new Dictionary<Type, StateInst>();
        public Dictionary<System.Type, List<StateInst>> CheckStateList = new Dictionary<Type, List<StateInst>>();
        public List<StateInst> AllStates => StateDic.Values.ToList();
        public List<StateInst> AllEnableStates => StateDic.Values.Where((st) => st.Enabled).ToList();

    }

    public class StateInst
    {
        public GameObject gameObject;
        public List<System.Type> activeCondition_Exist = new List<Type>();
        public List<System.Type> activeCondition_Except = new List<Type>();
        public Action OnEnable;
        public Action OnDisable;
        bool enabled;
        public bool Enabled => enabled;

        List<StateInst> AllStates => stateDateDic[gameObject].AllStates;
        List<StateInst> CheckStateList => stateDateDic[gameObject].CheckStateList.TryGetValue(this.GetType());
        List<System.Type> AllActiveStates => stateDateDic[gameObject].AllEnableStates.Select((x) => x.GetType()).ToList();






        private void UpdateStates(List<StateInst> states)
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


        public static StateInst GetStateInst<T>(GameObject gameObject) where T : StateInst, new()
        {
            StateDate stateDate = GlobalDate.stateDateDic.GetOrCreate(gameObject);
            StateInst state = stateDate.StateDic.GetOrCreate(typeof(T), new T());
            state.gameObject = gameObject;

            BasicEvent.OnDestroyEvent.Add(gameObject, state.OnObjectDestroyAction);

            return state;
        }
        public static bool GetState<T>(GameObject gameObject) where T : StateInst, new()
        {
            StateDate stateDate = GlobalDate.stateDateDic.TryGetValue(gameObject);
            if (stateDate == null) return false;
            StateInst state = stateDate.StateDic.TryGetValue(typeof(T));
            if (state == null) return false;
            return state.Enabled;
        }

        public static void SetState<T>(GameObject gameObject, bool enabled) where T : StateInst, new()
        {
            StateInst state = GetStateInst<T>(gameObject);
            if (enabled)
            {
                state.Enable();
            }
            else
            {
                state.Disable();
            }

        }
        public static void SetStateCondition<T>(GameObject gameObject, List<System.Type> exist, List<System.Type> except) where T : StateInst, new()
        {
            StateInst state = GetStateInst<T>(gameObject);
            state.activeCondition_Exist = exist;
            state.activeCondition_Except = except;

            Dictionary<Type, List<StateInst>> checkList = stateDateDic[gameObject].CheckStateList;
            exist.ForEach((stateType) =>
            {
                List<StateInst> states = checkList.GetOrCreate(stateType);
                states.AddNotHas(state);
            });


            Dictionary<Type, List<StateInst>> onDisableCheck = stateDateDic[gameObject].CheckStateList;
            except.ForEach((stateType) =>
            {
                List<StateInst> states = onDisableCheck.GetOrCreate(stateType);
                states.AddNotHas(state);
            });
        }

        public static void SetStateAction<T>(GameObject gameObject, Action onEnable = null, Action onDisable = null) where T : StateInst, new()
        {
            StateInst state = GetStateInst<T>(gameObject);
            state.OnEnable += onEnable;
            state.OnDisable += onDisable;
        }



    }



    public class Fall : StateInst
    {

    }
    public class OnGround : StateInst
    {

    }



}

public static class StateF
{
    public static void AddStateCondition<T>(GameObject gameObject, List<System.Type> exist, List<System.Type> except) where T : StateBundle.StateInst, new()
    {
        StateBundle.StateInst.SetStateCondition<T>(gameObject, exist, except);
    }

    public static void SetState<T>(GameObject gameObject, bool enabled) where T : StateBundle.StateInst, new()
    {
        StateBundle.StateInst.SetState<T>(gameObject, enabled);

    }


    public static void AddStateAction<T>(GameObject gameObject, Action onEnable = null, Action onDisable = null) where T : StateBundle.StateInst, new()
    {
        StateBundle.StateInst.SetStateAction<T>(gameObject, onEnable, onDisable);
    }


    public static bool GetState<T>(GameObject gameObject) where T : StateBundle.StateInst, new()
    {
        return StateBundle.StateInst.GetState<T>(gameObject);
    }
}
