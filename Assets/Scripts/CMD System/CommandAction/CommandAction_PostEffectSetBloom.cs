using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_PostEffectSetBloom : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                var postEffects = TagFinder.PostEffect.Object;
                Volume volume = postEffects.GetComponent<Volume>();
                Bloom bloom;
                volume.profile.TryGet<Bloom>(out bloom);
                float[] vs = cl.ReadParams<float>();
                if (vs.Length == 0) return;

                volume.weight = vs[0];
            }




        }
    }

}
