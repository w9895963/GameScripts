using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BasicEvent
{
    namespace Component
    {
        public class UnityStartProgress : MonoBehaviour
        {
            public static Action onStart;
            public string sceneName = "OnLoad";

            void Start()
            {
                onStart?.Invoke();
                CommandFileBundle.FileReader.ReadAllScriptsThenAddToScene();
                SceneF.FindScene(sceneName)?.Build();
            }




        }
    }
}
