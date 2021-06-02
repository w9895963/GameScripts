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
        public class ItemToggle : MonoBehaviour
        {
            [SerializeField]
            private Text titleComponent;
            [SerializeField]
            private Toggle toggleComponent;
            public string Title { get => titleComponent.text; set => titleComponent.text = value; }
            public void AddToggleAction(UnityAction<bool> onToggle)
            {
                toggleComponent.onValueChanged.AddListener(onToggle);
            }
        }
    }


}