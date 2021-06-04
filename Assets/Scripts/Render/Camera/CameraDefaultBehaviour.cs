using System.Collections;
using System.Collections.Generic;
using Global;
using Global.CameraFunc;
using UnityEngine;

public class CameraDefaultBehaviour : MonoBehaviour
{
    public bool followPlayer = false;
    private void Start()
    {
        if (followPlayer)
        {
            CameraUtility.Follow(Player.Data.GameObject);
        }
    }

}

