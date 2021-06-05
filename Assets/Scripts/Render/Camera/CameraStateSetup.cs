using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CameraState
{
    public class CameraControl : StateBundle.StateInst
    {

    }
    public class CameraFollow : StateBundle.StateInst
    {

    }

    public class CameraStateSetup : MonoBehaviour
    {


        private void Awake()
        {
            CameraControl();
            CameraFollow();
        }

        private void CameraControl()
        {
            List<Type> exist = new List<Type>();
            List<Type> except = new List<Type>() { typeof(CameraFollow) };

            StateF.AddStateCondition<CameraControl>(gameObject, exist, except);
            Action onEnable = () =>
            {
                gameObject.AddComponent<CameraMouseDrag>();
                gameObject.AddComponent<CameraMouseZoom>();
            };
            Action onDisable = () =>
            {
                gameObject.GetComponent<CameraMouseDrag>().Destroy();
                gameObject.GetComponent<CameraMouseZoom>().Destroy();
            };
            StateF.AddStateAction<CameraControl>(gameObject, onEnable, onDisable);
        }
        private void CameraFollow()
        {
            List<Type> exist = new List<Type>();
            List<Type> except = new List<Type>() { typeof(CameraControl) };

            StateF.AddStateCondition<CameraFollow>(gameObject, exist, except);
            Action onEnable = () =>
            {
                Follow follow = gameObject.AddComponent<Follow>();
                follow.target = Player.Data.GameObject;
            };
            Action onDisable = () =>
            {
                gameObject.RemoveComponentAll<Follow>();
            };
            StateF.AddStateAction<CameraFollow>(gameObject, onEnable, onDisable);
        }
    }





}