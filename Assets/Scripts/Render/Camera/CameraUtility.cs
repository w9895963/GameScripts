using System.Collections;
using System.Collections.Generic;
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
                if (targetObj == null) { return; }
                GameObject camObj = Camera.main.gameObject;
                BasicEvent.OnFixedUpdate.Add(camObj, () =>
                {
                    Vector2 position = targetObj.GetPosition2d();
                    Vector2 vt = position - camObj.GetPosition2d();
                    float dist = vt.magnitude;
                    float r1 = Curve.Evaluate(dist, 0.4f, 1f, 0, 1);

                    Vector2 vW = vt;
                    Vector2 v = vW * r1 * 8;
                    camObj.AddPosition(v * Time.fixedDeltaTime);
                });


            }

        }

    }

}