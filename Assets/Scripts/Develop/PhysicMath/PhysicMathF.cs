using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicMath;




public struct MathPh
{
    

    public static int TimeToStep(float time, float? deltaTime = null)
    {
        deltaTime = deltaTime != null ? deltaTime : Time.fixedDeltaTime;
        return (time / (float)deltaTime).CeilToInt();
    }

    public static float CalcDistance(float speed, float force, float mass = 1, float? deltaTime = null)
    {
        deltaTime = deltaTime != null ? deltaTime : Time.fixedDeltaTime;
        speed += force / mass * (float)deltaTime;
        return speed * (float)deltaTime;
    }

    public static float ChangeSpeedToForce(float startSpeed, float targetSpeed, float mass = 1, int step = 1, float? deltaTime = null)
    {
        float delV = targetSpeed - startSpeed;
        deltaTime = deltaTime != null ? deltaTime : Time.fixedDeltaTime;
        return delV / (float)deltaTime * mass / (float)step;
    }


    public static float DistanceToSpeed(float distance, float accelerate)
   => PhysicMath_DistanceToSpeed.DistanceToSpeed(distance, accelerate);

}
