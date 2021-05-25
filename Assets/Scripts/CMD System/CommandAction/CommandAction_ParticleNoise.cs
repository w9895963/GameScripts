using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ParticleNoise : CommandActionHolder
        {
            public override void Action(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystem par = obj.GetComponent<ParticleSystem>();
                if (par == null) { return; }
                ParticleSystem.NoiseModule noise = par.noise;
                float[] ps = cm.ReadParams<float>();
                if (ps.Length > 0)
                {
                    noise.frequency = ps[0];

                    if (ps.Length == 2)
                    {
                        noise.strength = new ParticleSystem.MinMaxCurve(ps[1]);
                    }
                    else
                    {
                        noise.strength = new ParticleSystem.MinMaxCurve(ps[1], ps[2]);
                    }
                }

            }




        }
    }

}
