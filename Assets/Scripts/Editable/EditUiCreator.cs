using System;
using System.Collections;
using System.Collections.Generic;
using UIBundle;
using UnityEngine;




namespace EditableBundle
{
    public static class ObjectEditUIBuilder
    {
        public static void Build(GameObject gameObject)
        {
            EditDate[] editDates = EditDateGenerator.CreateDateList.Create(gameObject);
            Table table = new Table();
            table.GetTable();
            table.DestroyAllItem();

            List<TableItem> tableItems = new List<TableItem>();
            var gen = new UIItemGenerate.BasicGen();
            editDates.ForEach((d) =>
            {
                tableItems.AddRange(gen.Generate(d));
            });

            table.AddItem(tableItems);

        }


    }






    namespace UIItemGenerate
    {
        public class BasicGen
        {
            public List<UIBundle.TableItem> Generate(EditDate date)
            {
                List<TableItem> re = new List<TableItem>();
                string title = date.Title;
                string[] paramNames = date.ParamNames;
                object[] contens = date.GetDate();
                if (!title.IsEmpty())
                {
                    var titleItem = new UIBundle.Item.TableItemTitle();
                    titleItem.text = title;
                    re.Add(titleItem);
                }
                contens.ForEach((ct, i) =>
                {
                    if (ct.IsType<float>())
                    {
                        var editItem = new UIBundle.Item.TableItemEditString();
                        editItem.content = ct.ToString();
                        editItem.title = paramNames[i];
                        editItem.onChanged = (str) =>
                        {
                            object[] d = new object[2];
                            d[i] = str.TryFloat();
                            date.ApplayDate(d);
                        };
                        re.Add(editItem);
                    }
                });


                return re;
            }
        }
    }

}