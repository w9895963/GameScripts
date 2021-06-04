using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditableBundle
{


    namespace DateType
    {
        public class EditableSetting : EditDate
        {
            PrefabBundle.Prefab prefab => PrefabI.EditableSetting;


            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "辅助设置",
                paramNames = new[] { "控制相机", "辅助图形" },
            };

            public override Type[] DateTypes => new[] { typeof(bool), typeof(bool) };
            public override System.Object[] GetDate()
            {
                EditorMode com = gameObject.GetComponent<EditorMode>();
                if (com == null) { return null; }

                bool indicatorShow = CamareF.IsIndicatorVisible();


                System.Object[] re = new System.Object[] { com.ControlCamera, indicatorShow };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                var p0 = (bool?)dates[0];

                if (p0 != null)
                {
                    EditorMode com = gameObject.GetComponent<EditorMode>();
                    com.ControlCamera = p0.Value;
                }


                var p1 = (bool?)dates[1];

                if (p1 != null)
                {
                    CamareF.SetIndicatorVisible(p1.Value);
                }



            }


        }
    }
}
