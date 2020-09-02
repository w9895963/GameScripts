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
        [ReadOnly] public IC_Base callBy;
        public DataStore shareData = new DataStore ();
        public TempObjects tempInstance = new TempObjects ();


        //* Class Definition
        [System.Serializable] public class DataStore {
            [SerializeField, ReadOnly] private List<DataStore.DataInstance> Data;
            private static List<DataInstance> list = new List<DataInstance> ();



            //* Public Method
            public DataInstance Get (string name, ShareDataType? type = null) {
                DataInstance item = list.Find ((x) => x.name == name);
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

            public void Add (string name, Vector2? ve2 = null, Object obj = null) {
                list.RemoveAll ((x) => x.name == name);
                DataInstance item = new DataInstance ();
                item.name = name;
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
            public void UpdateShareDateInterface () {
                Data = list;
            }


            //* Class Definition

            [System.Serializable] public class DataInstance {
                public ShareDataType type = ShareDataType.Mix;
                public string name;
                public bool boolData;
                public float floatData;
                public Vector2 vector2Data;
                public Object objectData;
            }

        }

        [System.Serializable] public class TempObjects {
            [SerializeField, ReadOnly] private Object[] instances;
            private List<Data> data = new List<Data> ();
            public class Data {
                public Object obj;
                public int? index;

                public Data (Object obj, int? index = null) {
                    this.obj = obj;
                    this.index = index;
                }
            }


            public void AddIfEmpty (int index, System.Func<Object> func) {
                Data obj = data.Find ((x) => x.index == index);
                if (obj == null) {
                    data.Add (new Data (func (), index));
                }
                UpdataInterface ();
            }
            public void AddIfEmpty (int index, Object obj) {
                Data item = data.Find ((x) => x.index == index);
                if (item == null) {
                    data.Add (new Data (obj, index));
                }
                UpdataInterface ();
            }
            public void Add (System.Func<Object> func) {
                data.Add (new Data (func ()));
                UpdataInterface ();
            }
            public void Add (Object obj) {
                data.Add (new Data (obj));
                UpdataInterface ();
            }
            public void Destroy (params int[] indexs) {
                if (indexs.Length > 0) {
                    indexs.ForEach ((i) => {
                        Data inst = data.Find ((x) => x.index == i);
                        if (inst != null) {
                            inst.obj.Destroy ();
                            data.Remove (inst);
                        }
                    });
                } else {
                    data.ForEach ((d) => {
                        d.obj.Destroy ();
                    });
                    data.Clear ();
                }

                UpdataInterface ();

            }

            private void UpdataInterface () {
                instances = data.Select (x => x.obj).ToList ().ToArray ();
            }
        }
    }

    [System.Serializable] public class Behaviours {
        public IC_Base[] Next = new IC_Base[1];
        public Action[] onFinish = new Action[1];
        public Action onStart = new Action ();
        public List<IC_Base> synchronization = new List<IC_Base> ();
        public Component[] dataConnect = new Component[0];
        public int actionIndex = 0;



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
    public void OnEnable () {
        BeforeEnable ();
        OnEnable_ ();
    }


    public void OnDisable () {
        OnDisable_ ();
        AfterDisable ();
        data.tempInstance.Destroy ();
    }


    public virtual void OnDisable_ () { }
    public virtual void OnEnable_ () { }

    private void BeforeEnable () {


        data.shareData.UpdateShareDateInterface ();


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
    private void AfterDisable () {
        int actionIndex = behaviour.actionIndex;
        if (actionIndex >= 0) {

            if (behaviour.Next.Length > actionIndex & behaviour.Next[actionIndex] != null) {
                behaviour.Next[actionIndex].enabled = true;
                behaviour.Next[actionIndex].data.callBy = this;
            }



            if (behaviour.onFinish.Length > actionIndex) {

                Call (behaviour.onFinish[actionIndex]);
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




}