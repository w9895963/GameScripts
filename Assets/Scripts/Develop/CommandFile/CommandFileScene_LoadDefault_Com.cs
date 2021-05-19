using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CommandFileBundle
{
    namespace Component
    {
        public class CommandFileScene_LoadDefault_Com : MonoBehaviour
        {

            void Start()
            {
                CommandFileBundle.FolderExecuter.ExecuteDefaultFolder();
            }


        }
    }
}