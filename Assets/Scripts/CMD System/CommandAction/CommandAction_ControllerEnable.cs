using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ControllerEnable : CommandLineActionHolder
        {
            const string prefabFolderPath = "CommandPrefab/Controller/";




            public override void Action(CommandLine cl)
            {
                foreach (var line in cl.commandFile.commandLines)
                {
                    if (line.Empty) continue;
                    string prefabName = line.title;
                    if (prefabName.IsEmpty()) continue;


                    string path = prefabFolderPath + prefabName;
                    string objNewName = $"{prefabName}-{cl.FileName}-{cl.lineIndex}";

                    GameObject ctlObj = GameObjectF.CreateFromPrefab(path, objNewName);
                    if (ctlObj == null) continue;


                    CommandFileBundle.Controller.Controller ctl = ctlObj.GetComponent<Controller.Controller>();
                    ctl.cl=line;
                    ctl.onUpdate += (d) =>
                    {
                        line.paramaters = d;
                        line.Execute();
                    };
                    ctl.onFinalUpdate += (d) =>
                    {
                        line.paramaters = d;
                        line.Execute();
                        line.WriteLine();
                    };

                    ctl.Setup(line.paramaters);
                }






            }




        }
    }

}
