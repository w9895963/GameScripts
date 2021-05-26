using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_LightAddGlobalLight : CommandActionHolder
        {
            const string pathHead = "CommandPrefab/Light/GlobalLight/Light";

            public override void Action(CommandLine cl)
            {
                int lightLayer = 0;
                if (cl.ParamsLength > 0)
                {
                    lightLayer = cl.ReadParam<int>(0);
                }

                string path = $"{pathHead} {lightLayer}";
                GameObject light = GameObjectF.CreateFromPrefab(path);
                cl.GameObject = light;

                SceneF.AddToScene(light, cl.SceneName);

            }




        }
    }

}
