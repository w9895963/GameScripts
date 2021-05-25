using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace Controller
    {
        public class Controller : MonoBehaviour
        {
            public CommandLine cl;
            public Action<string[]> onUpdate;
            public Action<string[]> onFinalUpdate;
            public virtual void Setup(string[] allParams) { }

        }
    }
}
