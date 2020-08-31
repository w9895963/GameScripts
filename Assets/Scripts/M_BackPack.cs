using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_BackPack : MonoBehaviour {
    public GameObject[] storage = new GameObject[1];
    public Test test = new Test ();
    private EventTrigger iconEvent;

    void Start () {
        EnableClick ();
    }




    //*Public
    public void PutinStorage (GameObject gameObject) {
        storage[0] = gameObject;

    }
    public void PutoutStorage (int index) {
        if (storage[index]) {
            storage[index].GetComponent<I_InPack> ().enabled = false;;
            storage[index] = null;
        }
    }
    public bool Contain (GameObject gameObject) {
        return storage[0] == gameObject;
    }
    public bool IsFull () {
        return storage[0] != null;
    }
    public void EnableClick () {
        iconEvent.Destroy ();
        iconEvent = Gb.BackpackButton.Ex_AddInputToTrigger (EventTriggerType.PointerClick, (d) => {
            PutoutStorage (0);
        });
    }
    public void DisableClick () {
        iconEvent.Destroy ();
    }

#if UNITY_EDITOR
    private void OnValidate () {
        if (EditorApplication.isPlaying) {
            EditorApplication.delayCall += () => {
                if (test.putin) {
                    test.putin = false;
                    PutinStorage (test.target);
                }
                if (test.putout) {
                    test.putout = false;
                    PutoutStorage (0);
                }
            };

        }
    }
#endif


    [System.Serializable]
    public class Test {
        public bool putin = false;
        public bool putout = false;
        public GameObject target;
    }
}