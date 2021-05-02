using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BasicEvent.Component;




namespace BasicEvent
{
    namespace Component
    {
        public class UnityEvent_OnCollisionEnter2D : BaseClass
        {
            private void OnCollisionEnter2D(Collision2D other)
            {
                base.Invoke(BasicEvent.EventType.OnCollisionEnter2D, other);
            }

        }
    }

}
