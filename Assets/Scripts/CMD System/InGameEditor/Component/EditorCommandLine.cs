using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace CMDBundle
{
    namespace Editor
    {
        public class EditorCommandLine : MonoBehaviour
        {
            public Button hideButton;
            public InputField commandTitle;
            public CommandLine cl;

            const string paramPrefabPath = "CommandPrefab/Editor/CommandParam";
            const string linePrefabPath = "CommandPrefab/Editor/CommandLine";

            public GameObject paramsHolder => commandTitle.gameObject.GetParent();

            bool hide = false;

            private void Start()
            {
                RectTransform rect = gameObject.GetComponent<RectTransform>();
                hideButton?.onClick.AddListener(() =>
                {
                    if (hide)
                    {
                        hide = false;
                        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 100);
                        float height = UIF.ChildrenHeight(paramsHolder);
                        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height + 20);
                    }
                    else
                    {
                        hide = true;
                        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50);
                    }
                });


                if (!cl.Empty)
                {
                    commandTitle.text = cl.title;
                    List<string> paramaters = cl.paramaters;
                    for (int i = 0; i < paramaters.Count; i++)
                    {
                        GameObject pObj = paramsHolder.CreateChildFrom(paramPrefabPath);
                        CommandParameter pCom = pObj.GetComponent<CommandParameter>();
                        pCom.index = i;
                        pCom.cl = cl;
                        pCom.Build();
                      

                    }


                }


            }


            public static void BuildLine(CommandLine line, GameObject holder)
            {
                GameObject lineObject = holder.CreateChildFrom(linePrefabPath);

                // GameObject pref = ResourceLoader.Load<GameObject>(linePrefabPath);
                // RectTransform parent = holder.GetComponent<RectTransform>();
                // parent.pa
                // GameObject lineObject = GameObject.Instantiate(pref, parent);
                EditorCommandLine lineCom = lineObject.GetComponent<EditorCommandLine>();
                lineCom.cl = line;
            }


        }
    }
}
