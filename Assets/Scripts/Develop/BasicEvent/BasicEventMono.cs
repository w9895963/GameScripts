using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BasicEvent
{
    namespace Component
    {
        public class BasicEventMono : MonoBehaviour
        {
            public bool destroyed = false;
            public Action action;
            public Delegate action_;
            public int actionCount = 0;

            public void RunAction<T>(T date)
            {
                (action_ as Action<T>)?.Invoke(date);
            }
            public void RunAction()
            {
                action?.Invoke();
            }


        }
    }


}
