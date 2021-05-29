using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMDBundle
{
    namespace ActionComponent
    {
        public class CommandAction_SetPosition : CommandActionHolder
        {

            public override bool IsRealTimeAction => true;

            public override string[] ParamNames => new string[2] { "X", "Y" };


            public override void Action(CommandLine cl)
            {
                GameObject obj = cl.GameObject;

                float[] vs = cl.ReadParams<float>();
                if (vs.Length < 2) { return; }
                obj.SetPositionLo(new Vector2(vs[0], vs[1]));
                DateF.AddDate<Date.GameObject.Position, Vector2>(obj, obj.GetPosition2d());
            }






        }
    }

}
