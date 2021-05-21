using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransformSetter : MonoBehaviour
{
    public GameObject targetObject;
    public bool setPosition = false;
    public bool setRotation = false;
    public bool setScale = false;
    void Start()
    {
        ObjectDate.OnDateUpdate(gameObject, ObjectDateType.Position2D, (d) =>
        {
            if (setPosition)
            {
                Vector2 p = (Vector2)d;
                targetObject?.SetPosition(p);
                ObjectDate.UpdateData(targetObject, ObjectDateType.Position2D, targetObject.GetPosition2d());
            }

        });
        ObjectDate.OnDateUpdate(gameObject, ObjectDateType.Rotation1D, (d) =>
        {
            if (setRotation)
            {
                float p = (float)d;
                targetObject?.SetRotate(p);
                ObjectDate.UpdateData(targetObject, ObjectDateType.Rotation1D, targetObject.GetRotate1D());
            }

        });
        ObjectDate.OnDateUpdate(gameObject, ObjectDateType.Scale2D, (d) =>
        {
            if (setScale)
            {
                Vector2 p = (Vector2)d;
                targetObject?.SetScale(p);
                ObjectDate.UpdateData(targetObject, ObjectDateType.Scale2D, targetObject.GetScale2d());
            }

        });
    }


}
