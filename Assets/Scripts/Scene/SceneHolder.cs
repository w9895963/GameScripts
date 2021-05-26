using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CMDBundle;
using UnityEngine;



namespace SceneBundle
{
    public class SceneHolder : MonoBehaviour
    {
        public List<CommandFile> comandFiles = new List<CommandFile>();


        public void Build()
        {

            comandFiles.Sort((f) => f.runOrder);
            comandFiles.ForEach((f) =>
            {
                f.ExecuteLines();
            });

           

        }


    }


  
}
