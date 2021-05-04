using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{





    public static Vector2 ProjectOnPlane(this Vector2 vector, Vector2 normal)
    {
        return (Vector2)Vector3.ProjectOnPlane(vector, normal);
    }
    public static Vector2 Project(this Vector2 vector, Vector2 direction)
    {
        return (Vector2)Vector3.Project(vector, direction);
    }
    public static float ProjectToFloat(this Vector2 vector, Vector2 direction)
    {
        Vector2 vct = (Vector2)Vector3.Project(vector, direction);
        float result = vct.magnitude;
        if ((vct.normalized + direction.normalized).magnitude < 1)
            result *= -1;
        return result;
    }


    public static Vector2 ScreenToWold(this Vector2 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }
    public static Vector2 WoldToScreen(this Vector2 vector)
    {
        return Camera.main.WorldToScreenPoint(vector);
    }


    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * vector;
    }
    public static Vector2 Rotate(this Vector2 vector, Vector2 to)
    {
        return to.normalized * vector.magnitude;
    }
    public static Vector2 Rotate(this Vector2 vector, Vector2 from, Vector2 to)
    {
        return Quaternion.FromToRotation(from, to) * vector;
    }
    public static float Angle(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to);
    }


    public static Vector2 ClamMinOnDirection(this Vector2 vector, float min, Vector2 direction)
    {
        Vector2 vectorH = vector.Project(direction);
        Vector2 vectorV = vector.ProjectOnPlane(direction);
        Vector2 vectorHnorm = vectorH.Rotate(direction, Vector2.right);
        vectorHnorm.x = vectorHnorm.x.ClampMin(min);
        vectorH = vectorHnorm.Rotate(Vector2.right, direction);
        return vectorH + vectorV;
    }
    public static Vector2 ClampMax(this Vector2 vector, float max)
    {
        if (vector.magnitude >= max)
        {
            return vector.normalized * max;
        }
        else
        {
            return vector;
        }
    }


    public static bool IsSameSide(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to) < 90;
    }
    public static bool IsNotZero(this Vector2 vector)
    {
        return vector.magnitude != 0;
    }



    public static Vector3 ToVector3(this Vector2 vector, float z = 0)
    {
        return new Vector3(vector.x, vector.y, z);
    }


    //*Vector2?
    public static Vector2 ToVector2(this Vector2? vector)
    {
        return vector == null ? Vector2.zero : (Vector2)vector;
    }
    public static bool NotNull(this Vector2? vector)
    {
        return vector == null ? false : true;
    }
    public static bool IsNull(this Vector2? vector)
    {
        return vector == null ? true : false;
    }

    //*Vector3
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return (Vector2)vector;
    }
    public static Vector2 ToVector2(this Vector3? vector)
    {
        return vector != null ? (Vector2)vector : Vector2.zero;
    }






}
