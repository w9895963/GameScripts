using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PhysicMathBundle
{
    public class PhysicMath_DistanceToForce
    {
        public float targetDistance = 3;
        public float maxSpeed = 20;
        public float maxAddForce = 100;
        public float maxDeForce = 50;
        public float currSpeed = 0;
        public float mass = 1;

        public float deltaTime => Time.fixedDeltaTime;

        public float currDist = 0;
        public float remainDist;
        public float tarSpeed;

        public PhysicMath_DistanceToForce(float targetDistance, float maxSpeed, float maxForce = 100, float minForce = -50, float currSpeed = 0, float mass = 1)
        {
            this.targetDistance = targetDistance;
            this.maxAddForce = maxForce;
            this.maxDeForce = minForce;
            this.maxSpeed = maxSpeed;
            this.currSpeed = currSpeed;
            this.mass = mass;
        }

        public float calc()
        {
            remainDist = targetDistance - currDist;
            tarSpeed = PhysicMath.DistanceToSpeed(remainDist, maxDeForce.Abs() / mass);
            tarSpeed = tarSpeed.ClampMax(maxSpeed);
            float force = PhysicMath.SpeedToForce(currSpeed, tarSpeed, mass);
            force = force.ClampMax(maxAddForce);
            currSpeed += force * deltaTime / mass;
            currDist += currSpeed * deltaTime;

            return force;
        }


        public static float DistanceToForce(ref PhysicMath_DistanceToForce instance, float targetDistance, float maxSpeed, float maxForce = 100, float minForce = -50, float currSpeed = 0, float mass = 1)
        {
            PhysicMath_DistanceToForce obj = instance;
            if (instance == null)
                obj = new PhysicMath_DistanceToForce(targetDistance, maxSpeed, maxForce, minForce, currSpeed, mass);
            instance = obj;
            return obj.calc();
        }
    }
}

