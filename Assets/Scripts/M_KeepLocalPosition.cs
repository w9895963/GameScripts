using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_KeepLocalPosition : MonoBehaviour {
    public Vector2 positionSelf;

    private void Start () {
        positionSelf = transform.localPosition;
    }
    private void Update () {
        transform.localPosition = positionSelf;
    }
}