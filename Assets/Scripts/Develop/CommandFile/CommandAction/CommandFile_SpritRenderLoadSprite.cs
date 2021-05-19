using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class SpritRenderLoadSprite
        {

            public static void Act(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                var render = obj.GetComponent<SpriteRenderer>();
                string localPath = cm.ReadParam<string>(0);
                render.sprite = FileF.LoadSprite(FileF.GetFilePathInSameFolder(cm.Path, localPath));
            }
        }
    }
}
