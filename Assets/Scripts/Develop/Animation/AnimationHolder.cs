using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
public class AnimationHolder : MonoBehaviour
{
    public void ShotBullet()
    {
        IFunctionManager functionManager = GetComponentInParent<IFunctionManager>();
        if (functionManager == null)
        { return; }
        ShotFunc shotFunc = functionManager.Manager.GetFunction<ShotFunc>();
        if (shotFunc == null)
        { return; }
        shotFunc.ShotBullet();
    }

}