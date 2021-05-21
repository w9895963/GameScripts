using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BasicEvent
{
    namespace Component
    {
        public class UnityStartProgress : MonoBehaviour
        {
            public string sceneName = "OnLoad";

            void Start()
            {
                CommandFileBundle.FolderExecuter.ExecuteAllScripts();
                SceneF.FindScene(sceneName).Build();
            }

        }
    }
}
