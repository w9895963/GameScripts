using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Reset : MonoBehaviour {
    public GameObject moveto;
    public void Reset () {
        gameObject.transform.position = moveto.transform.position;
        GetComponent<FS_Gravity> ().SetGravityDirection (Vector2.down);
    }
}