using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IC_Base : MonoBehaviour {
    public void EmptyBehavior () {
        behaviour = new Behaviours ();
    }

    [ReadOnly] public Data data = new Data ();
    [ContextMenuItem ("Empty Behavior", "EmptyBehavior")]
    public Behaviours behaviour = new Behaviours ();

    [System.Serializable] public class Data {
        public IC_Base callBy;
        public List<Object> creates = new List<Object> ();
        public int exitIndex = -1;


        public void CallEventIfEmpty (int index, System.Func<Object> call) {
            bool empty = false;
            List<Object> evs = creates;
            if (evs.Count <= index) {
                empty = true;
            } else if (!evs[index]) {
                empty = true;
            }
            if (empty) {
                evs.Add (index, call ());
            }

        }
        public void DestroyEvents (params int[] indexs) {
            creates.Destroy (indexs);
        }

        public void SetExitIndex (int index) { exitIndex = index; }

    }

    [System.Serializable] public class Behaviours {
        public Action[] onFinish = new Action[1];
        public List<IC_Base> synchronization = new List<IC_Base> ();
        public Action onStart = new Action ();
        public Component[] dataConnect = new Component[0];


        [System.Serializable]
        public class Action {
            public IC_Base[] setEnable = new IC_Base[1];
            public IC_Base[] setDisable = new IC_Base[1];
            public Events other = new Events ();
            [System.Serializable]
            public class Events {
                public UnityEvent unityEvent = new UnityEvent ();

            }
        }


    }


    //***********************
    public void OnEnable () {
        EnableAction ();
        Begin ();
    }



    public void OnDisable () {
        DisableAction ();
        Exits ();
    }

    private void Begin () {
        behaviour.onStart.setDisable.ForEach ((comp) => {
            if (comp) {
                comp.enabled = false;
                comp.data.callBy = this;
            };
        });
        behaviour.onStart.setEnable.ForEach ((comp) => {
            if (comp) {
                comp.enabled = true;
                comp.data.callBy = this;
            };
        });
        behaviour.onStart.other.unityEvent.Invoke ();
        behaviour.synchronization.ForEach ((comp) => {
            if (comp) {
                comp.enabled = true;
                comp.data.callBy = this;
            };
        });

    }
    private void Exits () {
        if (data.exitIndex < 0) {
            foreach (var di in behaviour.onFinish) {
                Call (di);
            }
        } else {
            Call (behaviour.onFinish[data.exitIndex]);
        }

        behaviour.synchronization.ForEach ((comp) => {
            if (comp) {
                comp.enabled = false;
                comp.data.callBy = this;
            };
        });

        void Call (Behaviours.Action di) {
            di.setDisable.ForEach ((comp) => {
                if (comp) {
                    comp.enabled = false;
                    comp.data.callBy = this;
                };
            });
            di.setEnable.ForEach ((comp) => {
                if (comp) {
                    comp.enabled = true;
                    comp.data.callBy = this;
                };
            });
            di.other.unityEvent.Invoke ();
        }
    }




    public virtual void EnableAction () { }
    public virtual void DisableAction () { }


}