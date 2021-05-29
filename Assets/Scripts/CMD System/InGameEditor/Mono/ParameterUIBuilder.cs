using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CMDBundle
{
    namespace Editor
    {
        public class ParameterUIBuilder : MonoBehaviour
        {
            public Text title;
            public InputField content;
            public int index;
            public CommandLine cl;



            public void Initial()
            {
                InputField inp = content;
                var paramaters = cl.paramaters;
                inp.text = paramaters[index];
                inp.onValueChanged.AddListener((v) =>
                {
                    OnDateChanged(v);

                });
                inp.onEndEdit.AddListener((v) =>
                {
                    OnDateChanged(v);
                    cl.WriteLine();
                });


                CommandActionHolder actionHolder = cl.actionHolder;
                if (actionHolder == null) return;
                string[] paramNames = actionHolder.ParamNames;
                title.text = paramNames?[index] ?? title.text;
            }

            private void OnDateChanged(string v)
            {
                var paramaters = cl.paramaters;
                paramaters[index] = v;
                if (cl.actionHolder.IsRealTimeAction)
                    cl.Execute();
            }
        }
    }
}
