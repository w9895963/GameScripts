using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Global.ObjectDynimicFunction;
using UnityEngine;


namespace Global
{
    namespace ObjectDynimicFunction
    {
        public class StateFunc : IFunctionCreate
        {
            [System.Serializable]
            public class Data
            {
                public List<State> stateList = new List<State>();
            }
            private Data data;
            private System.Action onStateChangeAction;
            private List<(string state, System.Action action)> onStateAddActions = new List<(string state, System.Action action)>();
            private List<(string state, System.Action action)> onStateRemoveActions = new List<(string state, System.Action action)>();


            public void OnCreate(FunctionManager functionManager)
            {
                data = functionManager.GetData<Data>(this);
                onStateChangeAction += null;

            }


            // * ---------------------------------- 


            [System.Serializable]
            public class State
            {
                public string name;
                public System.Func<System.Object> dataMapper = () => null;

            }

            // * ----------------------------------  Core


            public void Add(string st, System.Func<System.Object> dataMapper = null)
            {

                bool exist = data.stateList.Exists((x) => x.name == st);
                if (exist)
                {
                    return;
                }

                State sta = new State();
                sta.name = st;
                if (dataMapper != null)
                    sta.dataMapper = dataMapper;
                data.stateList.Add(sta);

                onStateChangeAction.Invoke();

                var actionLists = onStateAddActions.FindAll((x) => x.state == st);
                actionLists.ForEach((x) => x.action());
            }
            public void Remove(string st)
            {
                State state = data.stateList.Find((x) => x.name == st);
                if (state != null)
                {
                    data.stateList.Remove(state);

                    onStateChangeAction.Invoke();

                    var actionLists = onStateRemoveActions.FindAll((x) => x.state == st);
                    actionLists.ForEach((x) => x.action());
                }

            }
            // * ---------------------------------- 
            public void Add(System.Enum st, System.Func<System.Object> dataMapper = null)
            {
                Add(st.ToString(), dataMapper);
            }

            public void Remove(System.Enum st)
            {
                Remove(st.ToString());
            }
            public void Remove(params string[] st)
            {
                if (st.Length > 0)
                {
                    st.ToList().ForEach((x) =>
               {
                   Remove(x);
               });
                }

            }
            public void Remove(params System.Enum[] st)
            {
                if (st.Length > 0)
                {
                    string[] vs = st.ToList().Select((x) => x.ToString()).ToList().ToArray();
                    Remove(vs);
                }

            }

            public bool HasNo(params string[] sts)
            {
                if (sts == null)
                {
                    return true;
                }
                if (sts.Length == 0)
                {
                    return true;
                }

                List<string> stateList = data.stateList.Select((x) => x.name).ToList();
                List<string> list = stateList.FindAll((x) => sts.Contains(x));
                return list.Count == 0;
            }
            public bool HasAll(params string[] sts)
            {
                bool re = true;
                if (sts == null)
                {
                    return true;
                }
                if (sts.Length == 0)
                {
                    return true;
                }
                sts.ForEach((m) =>
                {
                    bool v = data.stateList.Exists((x) => x.name == m);
                    if (!v)
                    {
                        re = false;
                    }
                });
                return re;
            }
            public bool HasNo(params System.Enum[] sts)
            {
                string[] vs = sts.ToList().Select((x) => x.ToString()).ToList().ToArray();
                return HasNo(vs);
            }
            public bool HasAll(params System.Enum[] sts)
            {
                string[] vs = sts.ToList().Select((x) => x.ToString()).ToList().ToArray();
                return HasAll(vs);
            }
            public void AddStateChangedAction(System.Action action)
            {
                onStateChangeAction += action;
            }
            public void RemoveStateChangedAction(System.Action action)
            {
                onStateChangeAction -= action;
            }
            public void AddStateAddAction(System.Enum state, System.Action action)
            {
                (string state, System.Action action) a = (state.ToString(), action);
                onStateAddActions.Add(a);

            }
            public void RemoveStateAddAction(System.Action action)
            {
                onStateAddActions.RemoveAll((x) => x.action == action);
            }
            public void AddStateRemoveAction(System.Enum state, System.Action action)
            {
                (string state, System.Action action) a = (state.ToString(), action);
                onStateRemoveActions.Add(a);

            }
            public void RemoveStateRemoveAction(System.Action action)
            {
                onStateRemoveActions.RemoveAll((x) => x.action == action);
            }
          


        }
    }
}

