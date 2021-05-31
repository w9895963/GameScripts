using System.Collections;
using System.Collections.Generic;
using PrefabBundle;
using UnityEngine;
using UnityEngine.UI;

namespace EditableBundle
{

    namespace Comp
    {
        public class CompTitle : MonoBehaviour
        {
            public Text titleTextComponent;
            public string titleText { set => titleTextComponent.text = value; get => titleTextComponent.text; }
        }
    }


}