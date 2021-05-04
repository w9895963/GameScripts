using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PhysicMathBundle
{
    public static class PhysicMath_DistanceToSpeed
    {
        public static float DistanceToSpeed(float distance, float accelerate)
        {
            return Mathf.Sqrt(2 * distance.ClampMin(0) * accelerate.ClampMin(0.0001f));
        }
    }
}
