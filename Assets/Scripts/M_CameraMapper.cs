using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CameraMapper : MonoBehaviour {
    public Camera targetCamera = null;

    private void Update () {

    }

    private void OnValidate () {
        MapCamera (targetCamera);
    }

    public void MapCamera (Camera targetCamera) {
        if (targetCamera)
            targetCamera.orthographicSize = GetComponent<Camera> ().orthographicSize;
    }
}