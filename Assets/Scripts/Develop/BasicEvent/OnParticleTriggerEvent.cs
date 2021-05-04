using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{



    public class OnParticleTriggerEvent : MonoBehaviour
    {
        private Action action;

        private void OnParticleTrigger()
        {
            if (action != null) { action(); }
        }




        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.GetOrCreate<OnParticleTriggerEvent>(gameObject, (com) =>
            {
                com.action += action;
            });
        }

        public static void Remove(GameObject gameObject, Action action)
        {
            BasicEvent.Method.RemoveCommon<OnParticleTriggerEvent>(gameObject, (com) =>
            {
                com.action -= action; 
                return com.action;
            });
        }


    }





}
