using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AutoDestroy : MonoBehaviour {
    [SerializeField] private float time;
    [SerializeField, ReadOnly] private float timeCreate;


    void Start () {
        timeCreate = Time.time;
    }

    void Update () {
        if (Time.time - timeCreate >= time) {
            Destroy (gameObject);
        }
    }

    private void OnEnable () {
        timeCreate = Time.time;
    }

    public void SetTime (float time) {
        this.time = time;
    }
}