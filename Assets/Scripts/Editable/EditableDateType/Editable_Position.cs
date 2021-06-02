using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditableBundle.DateType;
using System;

namespace EditableBundle
{


    namespace DateType
    {
        public class Editable_Position : EditDate
        {
            private const string ControllerPropertyName = "显示控制器";

            public override BuildUiConfig UiConfig => new BuildUiConfig()
            {
                title = "位置",
                paramNames = new[] { "X", "Y" },
            };
            public override GameObject[] UiObjects
            {
                get
                {
                    GameObject[] uiObjects = base.UiObjects;
                    GameObject tableItem = PrefabI.UI_EditorItem_Toggle.CreateInstance((obj) =>
                    {
                        Comp.ItemToggle com = obj.GetComponent<Comp.ItemToggle>();
                        com.Title = ControllerPropertyName;
                        com.AddToggleAction((on) =>
                        {
                            if (on)
                            {
                                GameObject cont = PrefabI.UI_Controller_Position.CreateInstance(gameObject);
                                var com = cont.GetComponent<Utility.PositionController>();
                                com.onDrag += () =>
                                {
                                    OnDateUpdate?.Invoke();
                                };
                                BasicEvent.OnDestroyEvent.Add(obj, () => cont.Destroy());
                            }
                            else
                            {
                                gameObject.GetComponentInChildren<Utility.PositionController>().gameObject.Destroy();
                            }
                        });
                    });
                    List<GameObject> list = uiObjects.ToList();
                    list.Add(tableItem);
                    uiObjects = list.ToArray();
                    return uiObjects;
                }
            }

            public override Type[] DateTypes => new[] { typeof(float), typeof(float) };
            public override System.Object[] GetDate()
            {
                Vector2 p = gameObject.GetPosition2dLo();
                System.Object[] re = new System.Object[] { p.x, p.y };
                return re;
            }

            public override void ApplayDate(System.Object[] dates)
            {
                gameObject.SetPositionLo((float?)dates[0], (float?)dates[1]);
            }




        }
    }
}
