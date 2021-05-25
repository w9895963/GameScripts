using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ParticleEmission : CommandActionHolder
        {
            public override void Action(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystem par = obj.GetComponent<ParticleSystem>();
                if (par == null) { return; }
                ParticleSystem.EmissionModule em = par.emission;
                float[] vs = cm.ReadParams<float>();
                if (vs.Length > 0)
                {
                    em.rateOverTime = new ParticleSystem.MinMaxCurve(vs[0]);
                }

            }




        }
    }

}
