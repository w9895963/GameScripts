using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SpritRenderLoadSprite : CommandLineActionHolder
        {
            public override void OnSceneBuild(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                if (obj == null) { return; }
                string localPath = cl.ReadParam<string>(0);
                string path = cl.Path;
                var render = obj.GetComponent<SpriteRenderer>();
                render.sprite = FileF.LoadSprite(FileF.GetFilePathInSameFolder(path, localPath));
            }




        }
    }

}
