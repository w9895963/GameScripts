using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SpritRenderLoadSprite : CommandActionHolder
        {
            const string path = "Material/2DCustom";

            public override void Action(CommandLine cl)
            {
                if (cl.ParamsLength == 0)
                {
                    return;
                }


                GameObject obj = cl.GameObject;
                if (obj == null) { return; }
                string localPath = cl.ReadParam<string>(0);
                string path = cl.Path;
                var render = obj.GetComponent<SpriteRenderer>();
                string folderPath = FileF.GetFolderPath(path);
                render.sprite = FileF.LoadSprite(FileF.GetFilePathInSameFolder(path, localPath));




                if (cl.ParamsLength == 1)
                {
                    return;
                }

                Material mat = render.sharedMaterial;
                mat = GameObject.Instantiate(mat);
                render.sharedMaterial = mat;
                string texFileName = cl.ReadParam<string>(1);
                string texFilePath = FileF.GetFilePathInSameFolder(cl.Path, texFileName);
                Texture2D tex = FileF.LoadTexture(texFilePath);
                mat.SetTexture("_NormalMap", tex);

            }




        }
    }

}
