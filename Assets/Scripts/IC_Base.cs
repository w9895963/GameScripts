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
    [HideInInspector] public Data data = new Data ();

    [HideInInspector] public Behaviours behaviour = new Behaviours ();
    [System.Serializable] public class Data {
        public IC_Base callBy;
        public ShareData shareData = new ShareData ();
        public TempObjects tempInstance = new TempObjects ();
        public int actionIndex = -1;


        //* Class Definition
        [System.Serializable] public class ShareData {
            [SerializeField, ReadOnly] private List<ShareData.DataInstance> Data;
            public List<IC_Base> shareWith = new List<IC_Base> ();
            private List<DataInstance> list = new List<DataInstance> ();



            //* Public Method
            public DataInstance Get (string name, ShareDataType? type = null) {
                DataInstance item = list.Find ((x) => x.name == name);
                if (item != null) {
                    switch (type) {
                        default : return null;
                        case ShareDataType.Mix:
                                return (item.type != ShareDataType.Mix) ? null : item;
                        case ShareDataType.Vector2:
                                return (item.type != ShareDataType.Vector2) ? null : item;
                        case ShareDataType.Object:
                                return (item.type != ShareDataType.Object) ? null : item;
                    }
                } else {
                    return null;
                }
            }

            public void Add (string name, Vector2? ve2 = null, Object obj = null) {
                var l = shareWith.Select ((x) => x.data.shareData.list).ToList ();
                l.Add (list);
                l.ForEach ((list) => {
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
                });



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
        public List<IC_Base> Next = new List<IC_Base> ();
        public List<Action> onFinish = new List<Action> ();
        public Action onStart = new Action ();
        public List<IC_Base> synchronization = new List<IC_Base> ();



        [System.Serializable] public class Action {
            public List<IC_Base> setEnable = new List<IC_Base> ();
            public List<IC_Base> setDisable = new List<IC_Base> ();
            public Events other = new Events ();
            [System.Serializable]
            public class Events {
                public UnityEvent unityEvent = new UnityEvent ();

            }

        }

    }

    public enum ShareDataType { Vector2, Object, Mix }



    //***********************

    public new bool enabled {
        get => base.enabled;
        set {
            Fn._.OrderRun (() => {
                if (base.enabled != value) {
                    if (value == true) {
                        RunOnEnable ();
                        base.enabled = true;
                    } else {
                        RunOnDisable ();
                        base.enabled = false;
                    }
                }
            });

        }
    }
    private void OnValidate () {
        if (enabled) {
            RunOnEnable ();
        } else {
            RunOnDisable ();
        }
    }




    public void RunOnEnable () {


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
    public void RunOnDisable () {
        data.tempInstance.Destroy ();
        int actionIndex = data.actionIndex;
        if (actionIndex >= 0) {



            if (behaviour.Next.Count > actionIndex) {
                if (behaviour.Next[actionIndex] != null) {
                    behaviour.Next[actionIndex].enabled = true;
                    behaviour.Next[actionIndex].data.callBy = this;
                }
            }



            if (behaviour.onFinish.Count > actionIndex) {

                Behaviours.Action di = behaviour.onFinish[actionIndex];
                if (di != null) {
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




            behaviour.synchronization.ForEach ((comp) => {
                if (comp) {
                    comp.enabled = false;
                    comp.data.callBy = this;
                };
            });

        }
    }




}