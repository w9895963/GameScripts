using System.Collections;
using System.Collections.Generic;
using Global.Physic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Global
{
    namespace CameraFunc
    {
        public static class CameraUtility
        {
            public static void Follow(GameObject targetObj)
            {
                GameObject camObj = Camera.main.gameObject;
                BasicEvent.OnFixedUpdate.Add(camObj, () =>
                {
                    Rigidbody2D self = camObj.GetComponent<Rigidbody2D>();
                    Rigidbody2D target = targetObj.GetComponent<Rigidbody2D>();

                    Vector2 position = target.position;
                    Vector2 vt = position - self.position;
                    float dist = vt.magnitude;
                    float r1 = Curve.Evaluate(dist, 0.4f, 1f, 0, 1);

                    Vector2 vW = vt;
                    self.velocity = vW * r1 * 8;
                });
               

            }

        }

    }

}