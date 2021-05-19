using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CommandFileBundle
{
    namespace Action
    {
        public static class Particle_TextureReplace
        {
            public static void Act(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystemRenderer render = obj.GetComponent<ParticleSystemRenderer>();
                string texFileName = cm.ReadParam<string>(0);
                string texFilePath = Path.Combine(Path.GetDirectoryName(cm.Path), texFileName).FixPath();
                Texture2D tex = FileF.LoadTexture(texFilePath);
                Material mat = render.sharedMaterial;
                Material newMat = GameObject.Instantiate(mat);
                newMat.SetTexture("Texture", tex);
                render.sharedMaterial = newMat;
            }
        }
    }
}
