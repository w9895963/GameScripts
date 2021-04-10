using System.Collections;
using System.Collections.Generic;
using Global.ObjectDynimicFunction;
using UnityEngine;


namespace Global
{
    namespace ObjectDynimicFunction
    {
        public class TemplateFunction : IFunctionCreate, ILateCreate

        {
            [System.Serializable]
            public class Data
            {
                public Setting setting = new Setting();
                [System.Serializable]
                public class Setting
                {
                }

                public Variables variables = new Variables();
                [System.Serializable]
                public class Variables
                {

                }

            }


            #region Basic Fields ------------
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data.Setting set;
            private Data.Variables vrs;
            #endregion
            // ** ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;
                
                Data data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;
                set = data.setting;
                vrs = data.variables;

            }


            public void LateCreate()
            {
            }
        }
    }
}

