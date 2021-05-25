using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_MoveWithCamera : CommandActionHolder
        {

            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;
                MoveWithCamera com = obj.AddComponent<MoveWithCamera>();

                float[] ps = cl.ReadParams<float>();
                if (ps.Length > 0)
                {
                    com.moveRate.x = ps[0];
                }
                if (ps.Length > 1)
                {
                    com.moveRate.y = ps[1];
                }


            }




        }
    }

}
