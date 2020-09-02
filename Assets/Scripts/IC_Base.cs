using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IC_Base : MonoBehaviour {
    public void EmptyBehavior () {
        behaviour = new Behaviours ();
    }
    //*********************Property
    public Data data = new Data ();
    [ContextMenuItem ("清空", "EmptyBehavior")]
    public Behaviours behaviour = new Behaviours ();
    [System.Serializable] public class Data {
        public IC_Base callBy;
        [SerializeField, ReadOnly] private List<Object> creates = new List<Object> ();
        [ReadOnly] public int actionIndex = 0;
        [SerializeField, ReadOnly] private List<DataStore.DataInstance> ShareData;
        public DataStore shareData = new DataStore ();

        //**************************
        public void CallIfEmpty (int index, System.Func<Object> call) {
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
        public void DestroyAllEvents (params int[] indexs) {
            if (indexs.Length > 0) {
                creates.Destroy (indexs);
            } else {
                creates.Destroy ();
            }

        }

        //********************
        public void UpdateShareDateInterface () {
            ShareData = DataStore.list;
        }

        public class DataStore {
            public static List<DataInstance> list = new List<DataInstance> ();




            //* Public Method
            public DataInstance Get (int index, ShareDataType? type = null) {
                DataInstance item = list.Find ((x) => x.index == index);
                switch (type) {
                    default : return null;
                    case ShareDataType.Mix:
                            return (item.type != ShareDataType.Mix) ? null : item;
                    case ShareDataType.Vector2:
                            return (item.type != ShareDataType.Vector2) ? null : item;
                    case ShareDataType.Object:
                            return (item.type != ShareDataType.Object) ? null : item;
                }
            }

            public void Add (int index, Vector2? ve2 = null, Object obj = null) {
                list.RemoveAll ((x) => x.index == index);
                DataInstance item = new DataInstance ();
                item.index = index;
                int count = 0;
                if (ve2 != null) {
                    item.vector2Data = (Vector2) ve2;
                    count += 1;
                    item.type = ShareDataType.Vector2;
                }
                if (obj != null) {
                    item.objectData = obj;
                    count += 1;
                    item.type = ShareDataType.Object;
                }
                if (count > 1) {
                    item.type = ShareDataType.Mix;
                }

                list.Add (item);
            }




            //* Class Definition

            [System.Serializable] public class DataInstance {
                public ShareDataType type = ShareDataType.Mix;
                public int index;
                public bool boolData;
                public float floatData;
                public Vector2 vector2Data;
                public Object objectData;
            }

        }
    }

    [System.Serializable] public class Behaviours {
        public IC_Base[] Next = new IC_Base[1];
        public Action[] onFinish = new Action[1];
        public Action onStart = new Action ();
        public List<IC_Base> synchronization = new List<IC_Base> ();
        public Component[] dataConnect = new Component[0];
        public bool actionWhenEnable = false;



        [System.Serializable] public class Action {
            public IC_Base[] setEnable = new IC_Base[1];
            public IC_Base[] setDisable = new IC_Base[1];
            public Events other = new Events ();
            [System.Serializable]
            public class Events {
                public UnityEvent unityEvent = new UnityEvent ();

            }

        }

        public T[] GetConnects<T> () where T : Component {
            List<T> list = new List<T> ();
            dataConnect.ForEach ((x) => {
                if (x.GetType () == typeof (T)) {

                    list.Add ((T) x);
                }
            });

            return list.Distinct ().ToList ().ToArray ();
        }
    }

    public enum ShareDataType { Vector2, Object, Mix }




    //***********************
    // public void OnEnable () {
    //     EnableAction ();
    //     Begin ();
    // }

    // public void OnDisable () {
    //     DisableAction ();

    //     Exits ();
    // }

    public new bool enabled {
        set {
            if (value == true) {
                Begin ();
                base.enabled = value;
            } else {
                base.enabled = value;
                Exits ();
            }
        }
        get => base.enabled;
    }

    private void Begin () {
        data.actionIndex = behaviour.actionWhenEnable?0: -1;
        data.UpdateShareDateInterface ();


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
        if (data.actionIndex >= 0) {

            if (behaviour.Next.Length > data.actionIndex) {
                behaviour.Next[data.actionIndex].enabled = true;
                behaviour.Next[data.actionIndex].data.callBy = this;
            }



            if (behaviour.onFinish.Length > data.actionIndex) {

                Call (behaviour.onFinish[data.actionIndex]);
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
    }



    //**********************
    public virtual void EnableAction () { }
    public virtual void DisableAction () { }




}