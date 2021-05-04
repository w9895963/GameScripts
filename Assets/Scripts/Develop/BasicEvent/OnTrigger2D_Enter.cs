using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{

    public class OnTrigger2D_Enter : MonoBehaviour
    {
        private Action<Collider2D> action;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (action != null) { action(other); }
        }







        public static void Add(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Method.GetOrCreate<OnTrigger2D_Enter>(gameObject, (com) =>
            {
                com.action += action;
            });
        }
        public static void Remove(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Method.RemoveCommon<OnTrigger2D_Enter>(gameObject, (com) =>
            {
                com.action -= action; 
                return com.action;
            });
        }

    }


}
