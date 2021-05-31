using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditableBundle.Comp;
using PrefabBundle;
using UIBundle;
using UnityEngine;




namespace EditableBundle
{

    public static class ObjectEditorBuilder
    {
        static Prefab TitlePrefab = PrefabI.UI_EditorItem_Title;
        static Prefab EditLinePrefab = PrefabI.UI_EditorItem_EditLine;
        private static GameObject contentHolder => ShareField.ContentHolder;

        static string toolName = "编辑物体";

        public static void BuildEditorFor(GameObject gameObject)
        {
            EditDate[] editDates = EditDateGenerator.CreateDateList.Create(gameObject);
            contentHolder.DestroyChildren();

            GameObject title = TitlePrefab.CreateInstance(contentHolder);
            CompTitle com = title.GetComponent<CompTitle>();
            com.titleText = toolName;

            editDates.ForEach(((editDate) =>
            {


                GameObject[] UiObjects = editDate.BuildUi;
                UiObjects.ForEach((obj) =>
                {
                    obj.SetParent(contentHolder, false);
                });

            }));


        }




        public static GameObject[] DefaultUiBuildMethod(EditDate editDate)
        {
            List<GameObject> re = new List<GameObject>();

            string titleText = editDate.BuildUiConfig.title;
            bool v = titleText.IsEmpty();
            GameObject title = TitlePrefab.CreateInstance();
            if (!v)
            {
                title.GetComponent<CompTitle>().titleTextComponent.text = titleText;
            }
            re.Add(title);



            string[] ns = editDate.BuildUiConfig.paramNames;
            object[] ds = editDate.GetDate();
            int count = ds.Length;
            ds.ForEach((d, i) =>
            {
                if (d.IsType<float>() | d.IsType<int>())
                {
                    GameObject line = EditLinePrefab.CreateInstance();
                    var com = line.GetComponent<MarkEditorContentItem_EditLine>();
                    string st;
                    bool v1 = ns.TryGet(i, out st);
                    if (v1) com.title.text = st;

                    com.content.text = d.ToString();
                    com.content.onValueChanged.AddListener((st) =>
                    {
                        System.Object[] da = new System.Object[count];
                        if (d.IsType<float>())
                        {
                            da[i] = st.TryFloat();
                        }
                        else if (d.IsType<int>())
                        {
                            da[i] = st.TryInt();
                        }
                        editDate.ApplayDate(da);
                    });
                    re.Add(line);

                }



            });
            return re.ToArray();
        }


    }







}