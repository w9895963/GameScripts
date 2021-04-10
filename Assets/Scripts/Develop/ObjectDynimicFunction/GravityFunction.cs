using System.Collections;
using System.Collections.Generic;
using Global.ObjectDynimicFunction;
using UnityEngine;


namespace Global
{
    namespace ObjectDynimicFunction
    {
        public class GravityFunction : IFunctionCreate
        {
            [System.Serializable]
            public class Data
            {
                public Setting setting;
                [System.Serializable]
                public class Setting
                {
                    public bool enabled = true;

                    public Vector2 gravity = new Vector2(0, -100);

                }


            }

            private FunctionManager fm;
            private Data data;
            private Data.Setting set;
            private GameObject gameObject;
            private Physic.ConstantForce gravityForceObject;

            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                data = fm.GetData<Data>(this);
                set = data.setting;
                gameObject = fm.gameObject;

                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                gravityForceObject = rigidbody.AddConstantForce(set.gravity, ForceCalc);
            }



            private void ForceCalc()
            {
                Vector2 gravity = set.gravity * set.enabled.ToFloat();
                gravityForceObject.Force = gravity;

            }



            public void Disable()
            {
                set.enabled = false;
            }
            public void Enable()
            {
                set.enabled = true;
            }


        }
    }
}

