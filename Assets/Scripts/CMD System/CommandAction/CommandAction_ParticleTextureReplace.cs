using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ParticleTextureReplace : CommandActionHolder
        {
            public override void Action(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystemRenderer render = obj.GetComponent<ParticleSystemRenderer>();
                string texFileName = cm.ReadParam<string>(0);
                string texFilePath = FileF.GetFilePathInSameFolder(cm.Path, texFileName);
                Texture2D tex = FileF.LoadTexture(texFilePath);
                Material mat = render.sharedMaterial;
                Material newMat = GameObject.Instantiate(mat);
                newMat.SetTexture("Texture", tex);
                render.sharedMaterial = newMat;
            }




        }
    }

}
