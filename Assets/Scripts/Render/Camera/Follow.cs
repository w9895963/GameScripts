using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;


    private void Start()
    {
        BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateFollow);
    }
    private void OnDestroy()
    {
        BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateFollow);
    }

    private void FixedUpdateFollow()
    {
        GameObject camObj = Camera.main.gameObject;
        Vector2 position = target.GetPosition2d();
        Vector2 vt = position - camObj.GetPosition2d();
        float dist = vt.magnitude;
        float r1 = dist.Map(0.4f, 1f, 0, 1); ;


        Vector2 vW = vt;
        Vector2 v = vW * r1 * 8;
        camObj.AddPosition(v * Time.fixedDeltaTime);
    }
}
