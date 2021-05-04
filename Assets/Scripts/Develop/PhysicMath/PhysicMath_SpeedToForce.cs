using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PhysicMathBundle
{
    public static class PhysicMath_SpeedToForce
    {
        public static float SpeedToForce(float currentSpeed, float targetSpeed, float mass)
        {
            float delv = targetSpeed - currentSpeed;
            return delv / Time.fixedDeltaTime * mass;
        }
    }
}

