using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CMDBundle
{
    namespace Editor
    {
        public class CommandParameter : MonoBehaviour
        {
            public int index;
            public CommandLine cl;



            public void Build()
            {
                InputField inp = gameObject.GetComponentInChildren<InputField>();
                var paramaters = cl.paramaters;
                inp.text = paramaters[index];
                inp.onValueChanged.AddListener((v) =>
                {

                });
                inp.onEndEdit.AddListener((v) =>
                {
                    paramaters[index] = v;
                    cl.WriteLine();
                });
            }
        }
    }
}
