using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEvent.Component;

namespace BasicEvent
{
    namespace Component
    {

        public class UnityEvent_OnCollisionExit2D : BaseClass
        {
            private void OnCollisionExit2D(Collision2D other)
            {
                Invoke(EventType.OnCollisionExit2D,other);
            }

        }
    }

   
}
