using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SceneBundle;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_CreateObject : CommandLineActionHolder
        {
            public bool addToScene = true;
            public CommandLine commandLine;
            public override void Action(CommandLine cl)
            {
                var obj = GameObject.Instantiate(gameObject);
                if (gameObject.name.Contains("刚体"))
                {
                    gameObject.name.Log();
                }

                CommandAction_CreateObject com = obj.GetComponent<CommandAction_CreateObject>();
                com.commandLine = cl;

                cl.GameObject = obj;

                if (addToScene)
                {
                    SceneF.AddToScene(obj, FileF.GetFolderName(cl.Path));
                }


                string[] parentNames = cl.ReadParams<string>();
                if (!parentNames.IsEmpty())
                {
                    CommandAction_CreateObject[] cs = GameObject.FindObjectsOfType<CommandAction_CreateObject>();
                    CommandAction_CreateObject o = cs.ToList().Find((x) => x.commandLine.Path.Contains(parentNames.ToList()));
                    if (o == null) { return; }
                    obj.SetParent(o);
                }


            }




        }
    }

}
