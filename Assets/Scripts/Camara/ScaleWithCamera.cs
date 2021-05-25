using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithCamera : MonoBehaviour
{
    public float scaleFactor = 8;
    void Start()
    {
        gameObject.SetScale(Camera.main.orthographicSize / scaleFactor);
        DateF.AddAction<Date.Camera.Size, float>(Camera.main, (d) =>
        {
            gameObject.SetScale(d / scaleFactor);
        });
    }


}
