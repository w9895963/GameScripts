using System;
using System.Collections;
using System.Collections.Generic;
using UIBundle;
using UnityEngine;
using EditableBundle.DateType;
using System.Linq;

namespace EditableBundle
{


    namespace Func
    {
        public static class DateListCreator
        {
            private static List<EditDate> AllEditDate => ShareDate.AllEditDate;

            public static EditDate[] Create(GameObject gameObject)
            {
                var allDate = AllEditDate;
                allDate.RemoveAll((x) =>
                {
                    x.gameObject = gameObject;
                    bool IsTestFall = !x.TestObject();
                    return IsTestFall;
                });
                return allDate.ToArray();

            }
        }
    }



    public abstract class EditDate
    {
        public GameObject gameObject;


        public abstract System.Object[] GetDate();
        public abstract void ApplayDate(System.Object[] date);

        public virtual bool TestObject()
        {
            object[] vs = GetDate();
            if (vs.IsEmpty())
            {
                return false;
            }
            if (vs[0] == null)
            {
                return false;
            }
            return true;
        }





        public virtual GameObject[] UiObjects => Func.Editor_ListAllEditableDate.DefaultUiBuildMethod(this);

        public virtual BuildUiConfig UiConfig => new BuildUiConfig();





        public virtual string UniName => this.GetType().ToString();
        public virtual string[] StringDates
        {
            get
            {
                IEnumerable<string> enumerable = GetDate().Select((x) =>
                {
                    if (x == null) { return null; }
                    return x.ToString();
                });
                return enumerable.ToArray();
            }
        }
    }



    public class BuildUiConfig
    {
        public string title;
        public string[] paramNames;
        public ParamConfig[] ParamConfigs;

        public class ParamConfig
        {
            public ParamUiItem UiType = ParamUiItem.DropList;
            public string[] dropListContents;
            public Func<int> dropListValue = () => 0;


        }
        public enum ParamUiItem
        {
            DropList,
        }
    }

}
