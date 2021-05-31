using System;
using System.Collections;
using System.Collections.Generic;
using UIBundle;
using UnityEngine;
using EditableBundle.DateType;



namespace EditableBundle
{



    namespace EditDateGenerator
    {

        public static class CreateDateList
        {
            private static List<SingleGen> AllCreator = new List<SingleGen>()
            {
                new PositionDateGen(),
                new ScaleDateGen(),
                new SortLayerDateGen(),
                new SpriteDateGen(),
            };
            public static EditDate[] Create(GameObject gameObject)
            {
                List<EditDate> list = new List<EditDate>();
                AllCreator.ForEach((ct) =>
                {
                    EditDate editDate = ct.TryCreate(gameObject);
                    if (editDate == null) return;
                    list.Add(editDate);
                });

                return list.ToArray();
            }


        }
        public class SingleGen
        {

            public EditDate TryCreate(GameObject gameObject)
            {
                EditDate editDate = EditDateGen(gameObject);
                editDate.gameObject = gameObject;
                return editDate;
            }

            public virtual EditDate EditDateGen(GameObject gameObject)
            {
                return null;
            }


        }


    }






    public abstract class EditDate
    {
        public GameObject gameObject;






        public virtual GameObject[] BuildUi => ObjectEditorBuilder.DefaultUiBuildMethod(this);
        public virtual BuildUiConfig BuildUiConfig => new BuildUiConfig();



        public abstract System.Object[] GetDate();

        public abstract void ApplayDate(System.Object[] date);



    }

    public class BuildUiConfig
    {
        public string title;
        public string[] paramNames;
    }
}
