using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
    public GameObject objt;
    public Vector3 p;

    private void OnValidate () {
        if (objt) objt.transform.position = p;
    }

}