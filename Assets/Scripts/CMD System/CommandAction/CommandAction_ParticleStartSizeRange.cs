using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ParticleStartSizeRange : CommandActionHolder
        {
            public override void Action(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystem par = obj.GetComponent<ParticleSystem>();
                if (par == null) { return; }
                ParticleSystem.MainModule main = par.main;
                float[] vs = cm.ReadParams<float>();
                Vector2 range = new Vector2(vs[0], vs[1]);
                ParticleSystem.MinMaxCurve startSize = new ParticleSystem.MinMaxCurve(range.x, range.y);
                main.startSize = startSize;
            }




        }
    }

}
