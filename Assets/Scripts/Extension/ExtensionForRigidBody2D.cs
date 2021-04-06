using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.Events;
using Global.Physic;
using System;

public static class ExtensionForRigidBody2D
{
    public static VelosityChanger VelosityChangeTo(this Rigidbody2D rigidBody, Vector2 targetVelosity,
     float maxForce = 10000f, Vector2 direction = default)
    {
        GameObject gameObject = rigidBody.gameObject;
        VelosityChanger velosityChanger = new VelosityChanger();
        velosityChanger.Function(rigidBody, targetVelosity, maxForce, direction);

        return velosityChanger;

    }

    public static SustainForce AddForceSustain(this Rigidbody2D rigidBody, Vector2 force, UnityAction callback = null)
    {
        return new SustainForce(rigidBody, force, callback);
    }

}

namespace Global
{
    namespace Physic
    {
        public class VelosityChanger
        {
            private Rigidbody2D rigidbody;
            private float mass;
            private Vector2 targetVelosity;
            private float maxForce;
            private bool keepVelosity;
            private Vector2 direction = default;

            public void Function(Rigidbody2D rigidbody, Vector2 targetVelosity, float maxForce,
            Vector2 direction = default, bool keepVelosity = true)
            {
                mass = rigidbody.mass;
                this.rigidbody = rigidbody;
                this.direction = direction;
                this.targetVelosity = targetVelosity;
                this.maxForce = maxForce;
                this.keepVelosity = keepVelosity;

                if (direction != default)
                {
                    this.targetVelosity = this.targetVelosity.Project(direction);
                }

                UnityEventPort.AddFixedUpdateAction(rigidbody.gameObject, 0, ApplyForce);

            }


            private Vector2 CalcForce()
            {
                Vector2 currV = rigidbody.velocity;
                if (direction != default)
                {
                    currV = currV.Project(direction);
                }
                Vector2 deltaV = targetVelosity - currV;
                Vector2 forceTotalNeed = deltaV / Time.fixedDeltaTime * mass;
                Vector2 outForce = forceTotalNeed.ClampMax(maxForce);
                return outForce;
            }

            private void ApplyForce(UnityEventPort.CallbackData data)
            {
                Vector2 force = CalcForce();
                rigidbody.AddForce(force);

                if (!keepVelosity)
                {
                    if (force.magnitude < maxForce)
                    {
                        UnityEventPort.RemoveAction(rigidbody.gameObject, ApplyForce);
                    }
                }

            }

            public void StopFunction()
            {
                if (rigidbody != null)
                    UnityEventPort.RemoveAction(rigidbody.gameObject, ApplyForce);
            }
        }

        public class SustainForce
        {
            private Vector2 force;
            private UnityAction callback;
            private Rigidbody2D rigidbody;

            public SustainForce(Rigidbody2D rigidbody, Vector2 force, UnityAction callback)
            {
                this.rigidbody = rigidbody;
                this.force = force;
                this.callback = callback;

                UnityEventPort.AddFixedUpdateAction(rigidbody.gameObject, 0, ApplyForce);
            }



            private void ApplyForce(UnityEventPort.CallbackData data)
            {
                if (callback != null) callback();
                rigidbody.AddForce(force);
            }
            public void SetForce(Vector2 force)
            {
                this.force = force;
            }

            public void Disable()
            {
                UnityEventPort.RemoveAction(rigidbody.gameObject, ApplyForce);

            }

        }
    }
}
