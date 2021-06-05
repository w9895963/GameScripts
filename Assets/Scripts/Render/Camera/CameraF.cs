using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CamareF
{
    public static void SetIndicatorVisible(bool visible = true)
    {
        Camera cam = Camera.main;
        if (visible)
        {
            cam.cullingMask = LayerF.AddMask(cam.cullingMask, Layer.Indicator);
        }
        else
        {
            cam.cullingMask = LayerF.RemoveMask(cam.cullingMask, Layer.Indicator);
        }
    }
    public static bool IsIndicatorVisible()
    {
        Camera cam = Camera.main;

        return LayerF.IsMask(cam.cullingMask, Layer.Indicator);

    }

    public static void FollowPlayer()
    {
        GameObject cam = Camera.main.gameObject;
        StateF.SetState<CameraState.CameraFollow>(cam, true);
    }
    public static void TakeControl()
    {
        GameObject cam = Camera.main.gameObject;
        StateF.SetState<CameraState.CameraControl>(cam, true);
    }


}
