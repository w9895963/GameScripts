using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_IngameEditorEnable : CommandActionHolder
        {
            public override void Action(CommandLine cl)
            {
                string localPath = cl.ReadParam<string>(0);
                CommandFile file = cl.commandFile.Find(localPath);
                Editor.EditorUIBuilder editor = EditorBuilder.Build(file, cl);
            }



        }
    }

}
