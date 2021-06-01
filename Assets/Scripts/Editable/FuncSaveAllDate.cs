using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditableBundle.Comp;
using UnityEngine;

namespace EditableBundle
{
    namespace Func
    {
        public static class SaveAllDate
        {
            private const string prefabDateName = "prefabPath";
            private const string LocalPath = "SaveDate";

            public static void Save()
            {
                DateContainer dateContainer = new DateContainer();
                CompEditableObject[] allObj = GameObject.FindObjectsOfType<Comp.CompEditableObject>();
                allObj.ForEach((obj) =>
                {
                    GameObject gameObject = obj.gameObject;
                    StringDateList stringDateList = new StringDateList();


                    stringDateList.list.Add((date) =>
                    {
                        string path = PrefabF.GetPath(gameObject);
                        date.name = prefabDateName;
                        date.date = new[] { path };
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
                string path = FileF.GetFullPathFromDataFolder(LocalPath);
                File.WriteAllText(path, v);


            }

            [System.Serializable]
            public class DateContainer
            {
                public List<StringDateList> list = new List<StringDateList>();
            }


            [System.Serializable]
            public class StringDateList
            {
                public List<StringDate> list;

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