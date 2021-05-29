using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace CMDBundle
{
    namespace Editor
    {
        public class CommandLineUIBuilder : MonoBehaviour
        {
            public InputField commandTitle;
            public GameObject dynamicHeight;
            public CommandLine cl;




            public void Initial()
            {
                ParameterUIBuilder commandParameter = gameObject.GetComponentInChildren<ParameterUIBuilder>();
                GameObject paramsHolder = commandParameter.gameObject.GetParent();


                if (!cl.Empty)
                {
                    commandTitle.text = cl.title;
                    commandTitle.onEndEdit.AddListener((str) =>
                    {
                        cl.title = str;
                        cl.WriteLine();
                    });





                    List<string> paramaters = cl.paramaters;

                    paramaters.ForEach((pr, i) =>
                    {
                        GameObject pObj = paramsHolder.CreateChild(commandParameter.gameObject);
                        ParameterUIBuilder pCom = pObj.GetComponent<ParameterUIBuilder>();
                        pCom.index = i;
                        pCom.cl = cl;
                        pCom.Initial();
                    });


                    dynamicHeight?.SetHeight(UIF.GetChildrenHeight(paramsHolder));


                    commandParameter.gameObject.SetActive(false);

                }


            }





        }
    }
}
