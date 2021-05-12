using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour
{
    public Vector2 originPosition;
    public Vector2 moveFactor = new Vector2(0.1f, 0.1f);

    void Start()
    {
        originPosition = transform.position;
    }

    void Update()
    {
        Vector2 camP = Camera.main.transform.position;
        Vector2 P = gameObject.GetPosition2d();
        Vector2 dP = originPosition - camP;
        dP.Scale(moveFactor);

        Vector2 targetP = camP + dP;
        gameObject.SetPosition(targetP);
    }
}
