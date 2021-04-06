using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    namespace AttackSystem
    {
        public static class AttackUtility
        {

            public static void Shoot(Vector2 position = default, float angle = 0)
            {
                ParticleSystem bullet = GameObject.FindObjectOfType<Bullet>().GetComponent<ParticleSystem>();
                ParticleSystem.EmitParams pam = new ParticleSystem.EmitParams();

                // pam.position = position.ToVector3 ();
                ParticleSystem.ShapeModule shape = bullet.shape;
                shape.rotation = new Vector3(0, 0, angle);
                shape.position = position.ToVector3();
                // pam.rotation3D = new Vector3 (90, 90, 90);
                // pam.axisOfRotation = new Vector3 (90, 90, 90);

                pam.applyShapeToPosition = true;
                bullet.Emit(pam, 6);
            }
        }
    }
}