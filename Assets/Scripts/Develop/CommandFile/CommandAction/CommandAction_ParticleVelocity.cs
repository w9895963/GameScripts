using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ParticleVelocity : CommandLineActionHolder
        {
            public override void OnSceneBuild(CommandLine cm)
            {
                GameObject obj = cm.GameObject;
                if (obj == null) { return; }
                ParticleSystem par = obj.GetComponent<ParticleSystem>();
                if (par == null) { return; }
                ParticleSystem.VelocityOverLifetimeModule vel = par.velocityOverLifetime;
                float[] vs = cm.ReadParams<float>();
                if (vs.Length == 2)
                {
                    vel.x = new ParticleSystem.MinMaxCurve(vs[0]);
                    vel.y = new ParticleSystem.MinMaxCurve(vs[1]);
                    vel.z = new ParticleSystem.MinMaxCurve(0);
                }
                else if (vs.Length == 4)
                {
                    vel.x = new ParticleSystem.MinMaxCurve(vs[0], vs[1]);
                    vel.y = new ParticleSystem.MinMaxCurve(vs[2], vs[3]);
                    vel.z = new ParticleSystem.MinMaxCurve(0, 0);
                }
            }




        }
    }

}
