using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_PostEffectSetBloom : CommandLineActionHolder
        {

            public override void Action(CommandLine cl)
            {
                PostEffects postEffects = GameObject.FindObjectOfType<PostEffects>();
                Volume volume = postEffects.GetComponent<Volume>();
                Bloom bloom;
                volume.profile.TryGet<Bloom>(out bloom);
                string[] vs = cl.ReadParams<string>();
                Setter.Set(vs[0], (v) => volume.weight = v);
            }




        }
    }

}
