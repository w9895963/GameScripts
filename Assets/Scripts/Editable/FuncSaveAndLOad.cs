using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EditableBundle.Comp;
using UnityEngine;

namespace EditableBundle
{
    namespace Func
    {
        public static class SaveAndLOad
        {
            private const string PrefabDateName = "prefabPath";
            private const string LocalPath = "SaveDate";

            public static string filePath => FileF.GetFullPathFromDataFolder(LocalPath);

            public static void Save()
            {
                DateContainer dateContainer = new DateContainer();
                CompEditableObject[] allObj = GameObject.FindObjectsOfType<Comp.CompEditableObject>();
                allObj.ForEach((obj) =>
                {
                    GameObject gameObject = obj.gameObject;
                    StringDateListHolder stringDateList = new StringDateListHolder();


                    stringDateList.list.Add((date) =>
                    {
                        PrefabBundle.Prefab prefab = PrefabF.GetPrefab(gameObject);

                        date.name = PrefabDateName;
                        date.date = new[] { prefab.folderPath, prefab.fileName };
                    });

                    EditDate[] editDates = Func.DateListCreator.Create(gameObject);
                    editDates.ForEach((d) =>
                    {
                        StringDate date = new StringDate();
                        date.name = d.UniName;
                        date.date = d.StringDates;
                        stringDateList.list.Add(date);
                    });

                    dateContainer.list.Add(stringDateList);
                });

                string v = JsonUtility.ToJson(dateContainer);
                File.WriteAllText(filePath, v);


            }
            public static void Load()
            {
                EditableF.EmptyEditTable();


                CompEditableObject[] allObjectOnScene = GameObject.FindObjectsOfType<Comp.CompEditableObject>();
                allObjectOnScene.ForEach((x) => x.gameObject.DestroyImmediate());
                string v = File.ReadAllText(filePath);
                List<StringDateListHolder> allObjectDate = JsonUtility.FromJson<DateContainer>(v).list;


                allObjectDate.ForEach((objDateHolder) =>
                {
                    PrefabBundle.Prefab prefab = objDateHolder.Prefab;
                    GameObject gameObject = prefab.CreateInstance();
                    List<(EditDate date, string[] stringDate)> editDates = objDateHolder.EditDates;
                    editDates.ForEach((datePair) =>
                    {
                        EditDate date = datePair.date;
                        object[] dateList = date.StringDateParse(datePair.stringDate);
                        date.gameObject = gameObject;
                        date.ApplayDate(dateList);
                    });


                });
            }

            [System.Serializable]
            public class DateContainer
            {
                public List<StringDateListHolder> list = new List<StringDateListHolder>();
            }


            [System.Serializable]
            public class StringDateListHolder
            {
                public List<StringDate> list = new List<StringDate>();
                public PrefabBundle.Prefab Prefab
                {
                    get
                    {
                        PrefabBundle.Prefab prefab = new PrefabBundle.Prefab();
                        prefab.folderPath = list[0].date[0];
                        prefab.fileName = list[0].date[1];
                        return prefab;
                    }
                }

                public List<(EditDate date, string[] stringDate)> EditDates
                {
                    get
                    {
                        List<(EditDate date, string[] stringDate)> re = new List<(EditDate date, string[] stringDate)>();
                        List<EditDate> allDate = ShareDate.AllEditDate;
                        Dictionary<string, EditDate> allDateDict = allDate.ToDictionary((x) => x.UniName);
                        list.Where((x, i) => i >= 1).ForEach((x) =>
                        {
                            EditDate editDate = allDateDict.TryGetValue(x.name);
                            if (editDate != null)
                            {
                                re.Add((editDate, x.date));
                            }
                        });

                        return re;
                    }
                }
            }
            [System.Serializable]
            public class StringDate
            {
                public string name;
                public string[] date;

            }
        }
    }

}