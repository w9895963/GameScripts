using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IC_SmallCore : MonoBehaviour {

    //*********************Property
    [HideInInspector] public Data data = new Data ();


    [System.Serializable] public class Data {
        public TempObjects tempInstance = new TempObjects ();

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
    private bool createByScript = true;



    //***********************

    public new bool enabled {
        get => base.enabled;
        set {
           Global.Function.Fn(this).OrderRun (() => {
                if (base.enabled != value) {
                    if (value == false) {
                        data.tempInstance.Destroy ();
                    }
                    base.enabled = value;
                }

            });

        }
    }


    public void Awake () {
        if (createByScript) {
            enabled = false;
        }
    }
    public void OnDestroy () {
        data.tempInstance.Destroy ();
    }


    public void Reset () {
        createByScript = false;
    }



}