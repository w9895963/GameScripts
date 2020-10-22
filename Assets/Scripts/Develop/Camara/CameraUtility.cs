using System.Collections;
using System.Collections.Generic;
using Global.Physic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Global.Physic.PhysicUtility;


namespace Global {
    namespace CameraFunc {
        public static class CameraUtility {
            public static void Follow (GameObject targetObj) {
                GameObject camObj = Camera.main.gameObject;
                AddPhysicAction (camObj, PhysicOrder.Movement, (a) => {
                    Rigidbody2D self = camObj.GetComponent<Rigidbody2D> ();
                    Rigidbody2D target = targetObj.GetComponent<Rigidbody2D> ();
                    Vector2 pointer = Pointer.current.position.ReadValue ();
                    float xScale = (pointer.x - Screen.width / 2f) / (float) Screen.width;
                    float yScale = (pointer.y - Screen.height / 2f) / (float) Screen.height;

                    float size = Camera.main.orthographicSize;
                    Vector2 position = target.position + new Vector2 (size * xScale * 1.5f, size * (0.3f + yScale));
                    Vector2 vt = position - self.position;
                    float dist = vt.magnitude;
                    float r1 = Curve.Evaluate (dist, 0.4f, 1f, 0, 1);

                    Vector2 vW = vt;
                    self.velocity = vW * r1 * 8;
                });

            }

        }

    }

}