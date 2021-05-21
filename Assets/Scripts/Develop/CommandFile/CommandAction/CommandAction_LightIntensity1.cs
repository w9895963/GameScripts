using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightLayer : CommandLineActionHolder
        {

            public override void OnSceneBuild(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                Light2D li = obj.GetComponent<Light2D>();
                var l = obj.GetComponent<Light>();

                string[] ps = cl.ReadParams<string>();
                ps.ForEach((str) =>
                {
                    // LayerF.AddMask(l.renderingLayerMask,)
                    // l.renderingLayerMask
                });


            }




        }
    }

}
