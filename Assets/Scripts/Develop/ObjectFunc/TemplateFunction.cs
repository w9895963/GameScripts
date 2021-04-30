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


            }


            #region Basic Fields ------------
            private FunctionManager functionManager;
            private GameObject gameObject;
            private Data data;
            #endregion
            // ** ---------------------------------- 


            public void OnCreate(FunctionManager functionManager)
            {
                this.functionManager = functionManager;
                var fm = functionManager;
                gameObject = fm.gameObject;

                data = fm.GetData<Data>(this);
                data = data == null ? new Data() : data;

            }


            public void LateCreate()
            {
            }
        }
    }
}

