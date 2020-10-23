using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global.ObjectData;

public class ObjectDataLibrary : MonoBehaviour {
    public List<DataInstance> data = new List<DataInstance> ();



}

namespace Global {

    public static class ObjectData {
        public static void Add (GameObject gameObject, string key, Object obj) {
            var comp = gameObject.GetComponent<ObjectDataLibrary> ();
            if (comp == null) {
                comp = gameObject.AddComponent<ObjectDataLibrary> ();
            }
            DataInstance dataInstance = comp.data.Find ((x) => x.key == key);
            if (dataInstance == null) {
                dataInstance = new DataInstance ();
                comp.data.Add (dataInstance);
            }
            dataInstance.key = key;
            dataInstance.obj = obj;

        }

        public static T Get<T> (GameObject gameObject, string key) where T : class {
            var comp = gameObject.GetComponent<ObjectDataLibrary> ();
            if (comp != null) {
                DataInstance data = comp.data.Find ((x) => x.key == key);
                if (data != null) {
                    if (typeof (T) == data.obj.GetType ()) {
                        return data.obj as T;
                    }
                }

            }
            return null;
        }

        [System.Serializable] public class DataInstance {
            public string key;
            public Object obj;
        }
    }
}