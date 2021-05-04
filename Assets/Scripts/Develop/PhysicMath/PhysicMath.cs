using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicMathBundle;




public static class PhysicMath
{
    public static float SpeedToForce(float currentSpeed, float targetSpeed, float mass)
   => PhysicMath_SpeedToForce.SpeedToForce(currentSpeed, targetSpeed, mass);


    public static float DistanceToSpeed(float distance, float accelerate)
   => PhysicMath_DistanceToSpeed.DistanceToSpeed(distance, accelerate);

    public static float DistanceToForce(ref PhysicMath_DistanceToForce instance, float targetDistance, float maxSpeed, float maxForce = 100, float minForce = -50, float currSpeed = 0, float mass = 1)
    => PhysicMath_DistanceToForce.DistanceToForce(ref instance, targetDistance, maxSpeed, maxForce, minForce, currSpeed, mass);


}
