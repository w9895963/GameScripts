using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{

    public class OnCollision2D_Exit : MonoBehaviour
    {
        private Action<Collision2D> action;
        private void OnCollisionExit2D(Collision2D other)
        {
            if (action != null) { action(other); }
        }







        public static void Add(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.GetOrCreate<OnCollision2D_Exit>(gameObject, (com) =>
            {
                com.action += action;
            });
        }
        public static void Remove(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.RemoveCommon<OnCollision2D_Exit>(gameObject, (com) =>
            {
                com.action -= action; 
                return com.action;
            });
        }

    }


}
