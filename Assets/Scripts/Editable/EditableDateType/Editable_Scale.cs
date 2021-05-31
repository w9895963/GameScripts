using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;

namespace EditableBundle
{
    namespace EditDateGenerator
    {
        public class ScaleDateGen : SingleGen
        {
            public override EditDate EditDateGen(GameObject gameObject)
            {
                if (!gameObject.HasComponent<Transform>()) return null;
                return new Editable_Scale();
            }
        }
    }

    namespace DateType
    {
        public class Editable_Scale : EditDate
        {
            public override BuildUiConfig BuildUiConfig => new BuildUiConfig()
            {
                title = "缩放",
                paramNames = new[] { "X", "Y" },
            };


            public override System.Object[] GetDate()
            {
                Vector2 p = gameObject.GetScale2dLo();
                System.Object[] re = new System.Object[] { p.x, p.y };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                gameObject.SetScaleLo((float?)dates[0], (float?)dates[1]);
            }


        }
    }
}
