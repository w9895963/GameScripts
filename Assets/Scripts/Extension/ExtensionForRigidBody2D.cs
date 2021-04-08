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
     float maxForce = 10000f, Vector2 direction = default, System.Action beforeApplyForce = null)
    {
        GameObject gameObject = rigidBody.gameObject;
        VelosityChanger velosityChanger = new VelosityChanger();
        velosityChanger.Function(rigidBody, targetVelosity, maxForce, direction);
        velosityChanger.updaAction = beforeApplyForce;

        return velosityChanger;

    }

    public static Global.Physic.ConstantForce AddConstantForce(this Rigidbody2D rigidBody, Vector2 force, UnityAction beforeApplyForce = null)
    {
        return new Global.Physic.ConstantForce(rigidBody, force, beforeApplyForce);
    }

}

namespace Global
{
    namespace Physic
    {
        public class VelosityChanger
        {
            public Rigidbody2D rigidbody;
            public float mass;
            public Vector2 targetVelosity;
            public float maxForce;
            public bool keepVelosity;
            public Vector2 direction = default;
            public Action updaAction;

            public void Function(Rigidbody2D rigidbody, Vector2 targetVelosity, float maxForce,
            Vector2 direction = default, bool keepVelosity = true)
            {
                mass = rigidbody.mass;
                this.rigidbody = rigidbody;
                this.direction = direction;
                this.targetVelosity = targetVelosity;
                this.maxForce = maxForce;
                this.keepVelosity = keepVelosity;



                UnityEventPort.AddFixedUpdateAction(rigidbody.gameObject, 0, ApplyForce);

            }


            private Vector2 CalcForce()
            {
                Vector2 currV = rigidbody.velocity;
                if (direction != default)
                {
                    this.targetVelosity = this.targetVelosity.Project(direction);
                    currV = currV.Project(direction);
                }

                Vector2 deltaV = targetVelosity - currV;
                Vector2 forceTotalNeed = deltaV / Time.fixedDeltaTime * mass;
                Vector2 outForce = forceTotalNeed.ClampMax(maxForce);
                return outForce;
            }

            private void ApplyForce(UnityEventPort.CallbackData data)
            {
                if (updaAction != null)
                {
                    updaAction();
                }
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

            // * ---------------------------------- 

            public void StopFunction()
            {
                if (rigidbody != null)
                    UnityEventPort.RemoveAction(rigidbody.gameObject, ApplyForce);
            }
        }

        public class ConstantForce
        {
            private Vector2 force;
            private UnityAction callback;
            private Rigidbody2D rigidbody;

            public ConstantForce(Rigidbody2D rigidbody, Vector2 force, UnityAction callback)
            {
                this.rigidbody = rigidbody;
                this.force = force;
                this.callback = callback;

                Enable();

            }

            private void ApplyForceAction(UnityEventPort.CallbackData data)
            {
                if (callback != null) callback();
                rigidbody.AddForce(force);
            }
            public Vector2 Force
            {
                get => force;
                set => force = value;
            }

            public void Disable()
            {
                UnityEventPort.RemoveAction(rigidbody.gameObject, ApplyForceAction);
            }
            public void Enable()
            {
                UnityEventPort.AddFixedUpdateAction(rigidbody.gameObject, 0, ApplyForceAction);
            }


        }
    }
}
