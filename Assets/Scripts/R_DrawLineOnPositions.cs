using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class R_DrawLineOnPositions : MonoBehaviour {
    [SerializeField]
    private GameObject[] objects = new GameObject[0];
    [SerializeField]
    private SpriteShapeController spriteShape;
    [SerializeField]
    private float length = 0.5f;

    private void Start () {

    }


    void Update () {

        Spline spline = spriteShape.spline;
        spline.Clear ();
        for (int i = 0; i < objects.Length; i++) {
            Vector3 p = objects[i].transform.position;
            spline.InsertPointAt (i, transform.InverseTransformPoint (p));
            spline.SetTangentMode (i, ShapeTangentMode.Continuous);

            float rotate = objects[i].transform.rotation.eulerAngles.z;
            Vector3 tangentDir = Vector3.zero;
            if (i > 0 & i < objects.Length - 1) {
                Vector3 v1 = objects[i - 1].transform.position - objects[i].transform.position;
                Vector3 v2 = objects[i + 1].transform.position - objects[i].transform.position;
                float angle = Vector3.SignedAngle (v1, v2, Vector3.forward);
                tangentDir = Quaternion.AngleAxis (angle / 2 + 90 * Mathf.Sign (-angle), Vector3.forward) * v1;
            }


            spline.SetLeftTangent (i, tangentDir.normalized * length);
            spline.SetRightTangent (i, tangentDir.normalized * -1 * length);

        }


    }

    private void Reset () {
        if (spriteShape == null) spriteShape = GetComponent<SpriteShapeController> ();
    }



}