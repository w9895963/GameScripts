using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_MoveWithCamera : CommandLineActionHolder
        {

            public override void OnSceneBuild(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                MoveWithCamera com = obj.AddComponent<MoveWithCamera>();

                float[] ps = cl.ReadParams<float>();
                if (ps.Length > 0)
                {
                    com.moveFactor.x = ps[0];
                }
                if (ps.Length > 1)
                {
                    com.moveFactor.y = ps[1];
                }


            }




        }
    }

}
