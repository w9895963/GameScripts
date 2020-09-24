using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class M_CameraProperty : MonoBehaviour {
    public Camera target;
    public float size;
    private void OnValidate () {
        GetComponent<Camera> ().orthographicSize = size;
        if (target) target.orthographicSize = GetComponent<Camera> ().orthographicSize;
    }

    private void Reset () {
        size = GetComponent<Camera> ().orthographicSize;
    }

}