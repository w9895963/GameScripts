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
        public class CompItemSelection : MonoBehaviour
        {
            [SerializeField]
            private Text titleComponent;
            [SerializeField]
            private Dropdown dropdownComponent;
            public string Title { get => titleComponent.text; set => titleComponent.text = value; }
            public void SetSelection(string[] options, int currentOption, UnityAction<int> onSelection)
            {
                List<Dropdown.OptionData> ops = options.Select((x) => new Dropdown.OptionData(x)).ToList();
                dropdownComponent.options = ops;
                dropdownComponent.value = currentOption;
                dropdownComponent.onValueChanged.AddListener(onSelection);
            }
        }
    }


}