using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class M_BackPack : MonoBehaviour {
    public GameObject[] storage = new GameObject[1];
    public Test test = new Test ();
    void Start () {

    }

    void Update () {

    }

    //*Public
    public void PutinStorage (GameObject gameObject) {
        storage[0] = gameObject;
        gameObject.gameObject.SetActive (false);

    }
    public void PutoutStorage (GameObject gameObject, Vector2 position = default) {
        if (storage[0] == gameObject) {
            gameObject.SetActive (true);
            if (position != default) {
                gameObject.transform.Set2dPosition (position);
            } else {
                gameObject.transform.Set2dPosition (Gb.MainCharactor.transform.position);
            };
            storage[0] = null;
        }
    }
    public bool Contain (GameObject gameObject) {
        return storage[0] == gameObject;
    }
    public bool IsFull () {
        return storage[0] != null;
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
                    PutoutStorage (test.target, test.position);
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
        public Vector2 position;
    }
}