using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_CreateObject : CommandLineActionHolder
        {
            public bool addToMainObject = true;
            public bool addToScene = true;
            public bool parentToObject = false;
            public override void OnSceneBuild(CommandLine cl)
            {
                var obj = GameObject.Instantiate(gameObject);
                if (addToMainObject)
                {
                    cl.GameObject = obj;
                }
                if (addToScene)
                {
                    SceneF.AddToScene(obj, FileF.GetFolderName(cl.Path));
                }
                if (parentToObject)
                {
                    obj.SetParent(cl.GameObject);
                }


                float[] pas = cl.ReadParams<float>();
                if (pas.Length >= 2)
                {
                    obj.SetPosition(pas[0], pas[1]);
                }
                if (pas.Length >= 4)
                {
                    obj.SetScale(pas[2], pas[3]);
                }
                if (pas.Length >= 5)
                {
                    obj.SetRotation(pas[4]);
                }

            }




        }
    }

}
