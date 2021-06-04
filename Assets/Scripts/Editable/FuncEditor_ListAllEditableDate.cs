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

                editDates = SortBySameTitle(editDates);

                editDates.ForEach(((editDate, i) =>
                {
                    GameObject[] UiItems = editDate.UiObjects;

                    bool prevTitleNotSame = !PrevTitleSameTest(editDates, i);
                    if (prevTitleNotSame)
                    {
                        UiItems[0].SetParent(contentHolder, false);
                    }
                    else
                    {
                        UiItems[0].Destroy();
                    }

                    UiItems.Where((x, i) => i >= 1).ForEach((obj) =>
                    {
                        obj.SetParent(contentHolder, false);
                    });

                }));

                static EditDate[] SortBySameTitle(EditDate[] editDates)
                {
                    List<EditDate> ds = editDates.ToList();
                    List<string> titleCollection = new List<string>();
                    var list = ds.Select((x) =>
                    {
                        string tit = x.UiConfig.title;
                        bool hasTitle = titleCollection.Contains(tit);
                        if (!hasTitle)
                        {
                            titleCollection.Add(tit);
                        }
                        return titleCollection.Count;
                    });
                    ds.SortBy(list);
                    editDates = ds.ToArray();

                    return editDates;
                }

                static bool PrevTitleSameTest(EditDate[] editDates, int i)
                {
                    bool re = false;
                    if (i > 0)
                    {
                        if (editDates[i - 1].UiConfig.title == editDates[i].UiConfig.title)
                        {
                            re = true;
                        }
                    }

                    return re;
                }
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
                object[] allDate = editDate.GetDate();
                BuildUiConfig.ParamConfig UiConfig = editDate.UiConfig.ParamConfigs?[i];
                var date = allDate[i];
                int count = allDate.Length;
                GameObject re = null;
                if (UiConfig == null)
                {
                    re = AutoBuildUi();
                }
                else
                {
                    if (UiConfig.UiType == BuildUiConfig.ParamUiItem.DropList)
                    {
                        re = BuildDropList();
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

                        com.content.text = editDate.GetDate()[i].ToString();
                        editDate.OnDateUpdate += () =>
                        {
                            com.content.text = editDate.GetDate()[i].ToString();
                        };
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
                    else if (date.IsType<bool>())
                    {
                        re = PrefabI.UI_EditorItem_Toggle.CreateInstance();
                        var com = re.GetComponent<ItemToggle>();
                        string st;
                        bool v1 = ns.TryGet(i, out st);
                        if (v1) com.Title = st;
                        com.AddToggleAction((b) =>
                        {
                            System.Object[] dat = new System.Object[count];
                            dat[i] = b;
                            editDate.ApplayDate(dat);
                        });
                    }

                    return re;
                }

                GameObject BuildDropList()
                {
                    return PrefabI.UI_EditorItem_Selection.CreateInstance((obj) =>
                    {
                        CompItemSelection com = obj.GetComponent<Comp.CompItemSelection>();
                        com.Title = parmName;
                        int value = UiConfig.dropListValue?.Invoke() ?? 0;
                        com.SetSelection(UiConfig.dropListContents, value, (ind) =>
                        {
                            System.Object[] dates = new System.Object[2];
                            dates[i] = ind;
                            editDate.ApplayDate(dates);
                        });
                    });
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