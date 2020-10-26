using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Mods;
using UnityEngine;

public class ModSupport : MonoBehaviour, IModable {
    public bool saveToMod = false;

    public string ModTitle => ModUtility.GenerateTitle (this);

    public bool EnableWriteModDatas => saveToMod;

    public object ModableObjectData {
        get {
            Rigidbody2D ri = GetComponent<Rigidbody2D> ();
            data.drag = ri.drag;
            data.angularDrag = ri.angularDrag;
            data.bodyType = ri.bodyType;
            return data;
        }
    }

    public void LoadModData (ModObjectData data1) {
        Data data = JsonUtility.FromJson<Data> (data1.objectJson);
        Rigidbody2D ri = GetComponent<Rigidbody2D> ();
        ri.drag = data.drag;
        ri.angularDrag = data.angularDrag;
        ri.bodyType = data.bodyType;    
    }

    public Data data = new Data ();
    [System.Serializable]
    public class Data {
        public float drag;
        public float angularDrag;
        public RigidbodyType2D bodyType;
    }


}