using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{



    public class OnDestroyEvent : MonoBehaviour
    {
        private Action action;
        private void OnDestroy()
        {
            if (action != null) { action(); }
        }




        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.GetOrCreate<OnDestroyEvent>(gameObject, (com) =>
            {
                com.action += action;
            });
        }
        public static void Remove(GameObject gameObject, Action action)
        {
            BasicEvent.Method.RemoveCommon<OnDestroyEvent>(gameObject, (com) =>
            {
                com.action -= action; 
                return com.action;
            });
        }


    }





}
