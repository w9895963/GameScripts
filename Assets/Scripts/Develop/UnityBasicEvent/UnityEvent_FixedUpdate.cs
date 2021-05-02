using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEvent.Component;
using System;

namespace BasicEvent
{
    namespace Component
    {

        public class UnityEvent_FixedUpdate : BasicEvent.Component.BaseClass
        {
            private void FixedUpdate()
            {
                Invoke(BasicEvent.EventType.FixedUpdate);
            }



        }
    }




}