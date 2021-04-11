using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Global
{
    namespace ObjectDynimicFunction
    {

        public class FunctionManager
        {
            private List<FunctionData> functionList = new List<FunctionData>();
            public GameObject gameObject;


            public FunctionManager(GameObject gameObject)
            {
                this.gameObject = gameObject;
                UnityEventPort.AddOnDestroyAction(gameObject, 0, (d) =>
                {
                    functionList.ForEach((f) =>
                    {
                        if (f is IOnDestroy)
                        {
                            IOnDestroy f1 = (IOnDestroy)f;
                            f1.OnDestroy();
                        }
                    });
                });

            }

            public class FunctionData
            {
                public System.Type type;
                public System.Object function;
                public System.Object data;


            }




            public T CreateFunction<T>(System.Object data = null) where T : new()
            {
                T function = new T();

                FunctionData f = new FunctionData();
                f.function = function;
                f.type = typeof(T);
                f.data = data;
                functionList.Add(f);

                if (function is IFunctionCreate)
                {
                    IFunctionCreate ini = (IFunctionCreate)function;
                    ini.OnCreate(this);
                }



                return function;
            }

            public void CallLateCreateAction()
            {
                functionList.ForEach((x) =>
                {
                    if (x.function is ILateCreate)
                    {
                        ILateCreate function = (ILateCreate)x.function;
                        function.LateCreate();
                    }
                });

            }


            public T GetFunction<T>()
            {
                FunctionData f = functionList.Find((x) => x.type == typeof(T));
                return f == null ? default : (T)f.function;
            }

            public T GetData<T>(System.Object function)
            {
                FunctionData variable = functionList.Find((x) => x.function == function);
                return (T)variable.data;
            }

            // * ---------------------------------- 
            public static FunctionManager GetFunctionManager(GameObject gameObject)
            {
                IFunctionManager ifun = gameObject.GetComponent<IFunctionManager>();
                if (ifun == null)
                { return null; }
                return ifun.Manager;
            }
            public static T GetFunction<T>(GameObject gameObject) where T : class
            {
                FunctionManager functionManager = GetFunctionManager(gameObject);
                if (functionManager == null)
                { return null; }
                return functionManager.GetFunction<T>();
            }
        }

    }
}
