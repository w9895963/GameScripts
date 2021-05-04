using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{
  


    public class OnFixedUpdate : MonoBehaviour
    {
        private Action action;
        private void FixedUpdate()
        {
            if (action != null) { action(); }
        }




        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.GetOrCreate<OnFixedUpdate>(gameObject, (com) =>
            {
                com.action += action;
            });
        }
        public static void Remove(GameObject gameObject, Action action)
        {
            BasicEvent.Method.RemoveCommon<OnFixedUpdate>(gameObject, (com) =>
            {
                com.action -= action; 
                return com.action;
            });
        }


    }





}
