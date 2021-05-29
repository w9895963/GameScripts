using System;
using System.Collections;
using System.Collections.Generic;
using UIBundle;
using UnityEngine;



namespace EditableBundle
{



    namespace EditDateGenerator
    {
        public static class CreateDateList
        {
            private static List<SingleGen> AllCreator = new List<SingleGen>()
            {
                new PositionDateGen()
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
        public class PositionDateGen : SingleGen
        {
            public override EditDate EditDateGen(GameObject gameObject)
            {
                if (!gameObject.HasComponent<Transform>()) return null;
                return new Editable_Position();
            }
        }
    }


    public class EditDate
    {
        public GameObject gameObject;
        public virtual string Title
        {
            get
            {
                (string title, string[] pars) names;
                TypeToNameDic.TryGetValue(this.GetType(), out names);
                return names.title;
            }
        }
        public virtual string[] ParamNames
        {
            get
            {
                (string title, string[] pars) names;
                TypeToNameDic.TryGetValue(this.GetType(), out names);
                return names.pars;
            }
        }


        public void Initial(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }




        public virtual System.Object[] GetDate()
        {
            return default;
        }

        public virtual void ApplayDate(System.Object[] date) { }





        public static Dictionary<System.Type, (string title, string[] pars)> TypeToNameDic = new Dictionary<Type, (string title, string[] pars)>
        {
            { typeof(Editable_Position), ("位置", new[] { "X", "Y" }) },
        };
    }


    public class Editable_Position : EditDate
    {


        public override System.Object[] GetDate()
        {
            Vector2 p = gameObject.GetPosition2dLo();
            System.Object[] re = new System.Object[] { p.x, p.y };
            return re;
        }

        public override void ApplayDate(System.Object[] dates)
        {
            gameObject.SetPositionLo((float?)dates[0], (float?)dates[1]);
        }


    }
    public class Editable_Scale : EditDate
    {



        public override System.Object[] GetDate()
        {
            Vector2 p = gameObject.GetScale2dLo();
            System.Object[] re = new System.Object[] { p.x, p.y };
            return re;
        }

        public override void ApplayDate(System.Object[] dates)
        {
            gameObject.SetScaleLo((float?)dates[0], (float?)dates[1]);
        }



    }





}
