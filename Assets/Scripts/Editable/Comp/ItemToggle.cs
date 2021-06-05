using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrefabBundle;
using UIBundle.Component;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace EditableBundle
{
    namespace Comp
    {
        public class ItemToggle : MonoBehaviour, IParams
        {
            [SerializeField]
            private Text titleComponent;
            [SerializeField]
            private Toggle toggleComponent;
            public string Title { get => titleComponent.text; set => titleComponent.text = value; }
            public bool Value { get => toggleComponent.isOn; set => toggleComponent.isOn = value; }
            public object[] Parameters
            {
                get
                {
                    object[] re = new object[2];
                    re[0] = Title;
                    re[1] = Value;
                    return re;
                }
                set
                {
                    value.TryGet(0, (string v) =>
                        Title = v
                    );
                    value.TryGet(1, (bool v) =>
                        Value = v
                    );
                    value.TryGet(2, (UnityAction<bool> v) =>
                        AddToggleAction(v)
                    );

                }
            }

            public void AddToggleAction(UnityAction<bool> onToggle)
            {
                toggleComponent.onValueChanged.AddListener(onToggle);
            }
        }
    }


}