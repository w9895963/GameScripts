using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDestroy : MonoBehaviour {
    public float time;
    public float timeCreate;
    // Start is called before the first frame update
    void Start () {
        timeCreate = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (Time.time - timeCreate >= time) {
            Destroy (gameObject);
        }
    }

    private void OnEnable () {
        timeCreate = Time.time;
    }
}