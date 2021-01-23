using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global.ObjectData;

public class ObjectDataLibrary : MonoBehaviour {
    public List<DataInstance> data = new List<DataInstance> ();



}

namespace Global {

    public static class ObjectData {
        public static DataInstance Add (GameObject gameObject, string key, System.Object obj) {
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

            return dataInstance;

        }

        public static DataInstance Get (GameObject gameObject, string key) {
            var comp = gameObject.GetComponent<ObjectDataLibrary> ();
            if (comp != null) {
                DataInstance data = comp.data.Find ((x) => x.key == key);
                if (data != null) {
                    return data;
                }
            }
            return null;
        }
        public static DataInstance Get (GameObject gameObject, string key, System.Object initialData) {
            DataInstance data = Get (gameObject, key);
            if (data == null) {
                data = Add (gameObject, key, initialData);
            }
            return data;
        }

        [System.Serializable] public class DataInstance {
            public string key;
            public System.Object obj;

            public T Read<T> () {
                return (T) System.Convert.ChangeType (obj, typeof (T));
            }
            public void Set (System.Object data) {
                obj = data;
            }
        }
    }
}