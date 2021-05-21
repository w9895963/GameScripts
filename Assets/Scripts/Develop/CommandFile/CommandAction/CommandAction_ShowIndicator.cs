using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_ShowIndicator : CommandLineActionHolder
        {
            public override void AfterSceneBuild(CommandLine cl)
            {
                CamareF.SetIndicatorVisible();

                if (cl.ParamsLength > 0)
                {
                    GameObject[] gameObjects = GameObjectF.GetObjectsInLayer(Layer.Indicator);
                    gameObjects.ForEach((x) =>
                    {
                        SpriteRenderer rn = x.GetComponent<SpriteRenderer>();
                        if (rn != null)
                        {
                            Color color = rn.color;
                            color.a = cl.ReadParam<float>(0);
                            rn.color = color;
                        }

                    });

                }
            }

        }
    }

}
