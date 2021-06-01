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
    namespace Func
    {
        public static class Editor_ListAllEditableDate
        {
            static Prefab TitlePrefab = PrefabI.UI_EditorItem_GroupTitle;
            static Prefab EditLinePrefab = PrefabI.UI_EditorItem_EditLine;
            static GameObject contentHolder => ShareDate.ContentHolder;

            static string toolName = "编辑物体";

            public static void BuildEditorFor(GameObject gameObject)
            {
                EditDate[] editDates = Func.DateListCreator.Create(gameObject);
                contentHolder.DestroyChildren();

                GameObject title = TitlePrefab.CreateInstance(contentHolder);
                CompGroupTitle com = title.GetComponent<CompGroupTitle>();
                com.titleText = toolName;

                editDates.ForEach(((editDate) =>
                {
                    GameObject[] UiObjects = editDate.UiObjects;
                    UiObjects.ForEach((obj) =>
                    {
                        obj.SetParent(contentHolder, false);
                    });

                }));


            }




            public static GameObject DefaultUiTitleBuildMethod(EditDate editDate)
            {

                string titleText = editDate.UiConfig.title;
                bool v = titleText.IsEmpty();
                GameObject title = TitlePrefab.CreateInstance();
                if (!v)
                {
                    title.GetComponent<CompGroupTitle>().titleTextComponent.text = titleText;
                }


                return title;
            }
            public static GameObject DefaultUiMemberBuildMethod(EditDate editDate, int i)
            {
                string[] ns = editDate.UiConfig.paramNames;
                string parmName = editDate.UiConfig.paramNames[i];
                object[] ds = editDate.GetDate();
                BuildUiConfig.ParamConfig cfg = editDate.UiConfig.ParamConfigs?[i];
                var date = ds[i];
                int count = ds.Length;
                GameObject re = null;
                if (cfg == null)
                {
                    re = AutoBuildUi();
                }
                else
                {
                    if (cfg.UiType == BuildUiConfig.ParamUiItem.DropList)
                    {
                        re = PrefabI.UI_EditorItem_Selection.CreateInstance((obj) =>
                        {
                            CompItemSelection com = obj.GetComponent<Comp.CompItemSelection>();
                            com.Title = parmName;
                            int value = cfg.dropListValue?.Invoke() ?? 0;
                            com.SetSelection(cfg.dropListContents, value, (ind) =>
                            {
                                System.Object[] dates = new System.Object[2];
                                dates[i] = ind;
                                editDate.ApplayDate(dates);
                            });
                        });
                    }
                }

                return re;

                GameObject AutoBuildUi()
                {
                    if (date.IsType<float>() | date.IsType<int>() | date.IsType<string>())
                    {
                        re = EditLinePrefab.CreateInstance();
                        var com = re.GetComponent<CompItemStringEditor>();
                        string st;
                        bool v1 = ns.TryGet(i, out st);
                        if (v1) com.title.text = st;

                        com.content.text = date.ToString();
                        com.content.onValueChanged.AddListener((st) =>
                        {
                            System.Object[] dat = new System.Object[count];
                            if (date.IsType<float>())
                            {
                                dat[i] = st.TryFloat();
                            }
                            else if (date.IsType<int>())
                            {
                                dat[i] = st.TryInt();
                            }
                            else if (date.IsType<string>())
                            {
                                dat[i] = st;
                            }
                            editDate.ApplayDate(dat);
                        });
                    }

                    return re;
                }
            }

          

            public static GameObject[] DefaultUiBuildMethod(EditDate editDate)
            {
                List<GameObject> re = new List<GameObject>();

                re.Add(DefaultUiTitleBuildMethod(editDate));


                string[] ns = editDate.UiConfig.paramNames;
                object[] ds = editDate.GetDate();
                int count = ds.Length;
                ds.ForEach((d, i) =>
                {
                    GameObject gameObject = DefaultUiMemberBuildMethod(editDate, i);
                    if (gameObject != null)
                    {
                        re.Add(gameObject); ;
                    }
                });
                return re.ToArray();
            }


        }
    }



}