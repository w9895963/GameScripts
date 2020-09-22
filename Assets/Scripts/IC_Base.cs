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




        //* Class Definition
        [System.Serializable] public class ShareData {
            [SerializeField] private GameObject storeObject;
            [SerializeField] private List<DataInstance> list = new List<DataInstance> ();
            public static Dictionary<GameObject, List<DataInstance>> globleData =
                new Dictionary<GameObject, List<DataInstance>> ();



            //* Public Method
            public DataInstance Get (string name) {
                if (globleData.ContainsKey (storeObject)) {
                    return globleData[storeObject].Find ((x) => x.name == name);
                } else {
                    return null;
                }


            }

            public void Add (string dataName, Vector2 vector2) {
                if (!globleData.ContainsKey (storeObject)) {
                    globleData.Add (storeObject, list);
                } else {
                    list = globleData[storeObject];
                }

                list.RemoveAll ((x) => x.name == dataName);


                DataInstance data = new DataInstance ();
                data.name = dataName;
                data.vector2Data = vector2;
                data.type = ShareDataType.Vector2;

                list.Add (data);

            }

            public void SetStoreObject (GameObject storeObject) {
                this.storeObject = storeObject;
                if (!globleData.ContainsKey (storeObject)) {
                    globleData.Add (storeObject, list);
                } else {
                    list = globleData[storeObject];
                }
            }



            //* Class Definition

            [System.Serializable] public class DataInstance {
                public ShareDataType type;
                public string name;
                public bool boolData;
                public float floatData;
                public Vector2 vector2Data;
                public Object objectData;
            }

        }

        [System.Serializable] public class TempObjects {
            [SerializeField] private List<Data> data = new List<Data> ();
            [System.Serializable] public class Data {
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

            }
            public void AddIfEmpty (int index, Object obj) {
                Data item = data.Find ((x) => x.index == index);
                if (item == null) {
                    data.Add (new Data (obj, index));
                }

            }
            public void Add (System.Func<Object> func) {
                data.Add (new Data (func ()));
            }
            public void Add (Object obj) {
                data.Add (new Data (obj));
            }
            public bool Has (int index) {
                return data.Count > index;
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


            }

        }
    }

    [System.Serializable] public class Behaviours {
        public List<Action> onFinish = new List<Action> ();
        public Action onStart = new Action ();


        //**
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

    public enum ShareDataType { Bool, Float, Vector2, Object }



    //***********************

    public new bool enabled {
        get => base.enabled;
        set {
            Global.Funtion.Fn (this).OrderRun (() => {
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




    public void RunOnEnable () {

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

    }
    public void RunOnDisable () {
        data.tempInstance.Destroy ();
    }

    public void RunFinishedAction (int actionIndex) {
        if (actionIndex >= 0) {



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




        }
    }



}