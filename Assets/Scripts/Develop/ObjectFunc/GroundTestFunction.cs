using System.Collections;
using System.Collections.Generic;
using Global;
using Global.ObjectDynimicFunction;
using UnityEngine;



namespace Global
{
    namespace ObjectDynimicFunction
    {
        public class GroundTestFunction : IFunctionCreate
        {
            [System.Serializable]
            public class Data
            {
                public bool onGround = false;
                public List<ContactObject> contactList = new List<ContactObject>();
            }
            private Data data;
            private GameObject gameObject;
            private StateFunc state;
            private FunctionManager fm;


            public void OnCreate(FunctionManager functionManager)
            {
                fm = functionManager;
                data = fm.GetData<Data>(this);
                gameObject = fm.gameObject;
                state = fm.GetFunction<StateFunc>();
                UnityEventPort.AddCollisionAction(gameObject, 0,
                                                 onCollisionEnter: OnCollisionEnter,
                                                 onCollisionExit: OnCollisionExit);
            }




            private void OnCollisionEnter(UnityEventPort.CallbackData data)
            {
                Collision2D other = data.collisionData;
                ContactObject contactObj = new ContactObject();
                contactObj.gameObject = other.gameObject;
                Vector2 normal = other.contacts[0].normal;
                contactObj.normal = normal;
                this.data.contactList.Add(contactObj);

                OnGroundTest();

            }

            private void OnCollisionExit(UnityEventPort.CallbackData data)
            {
                Collision2D other = data.collisionData;
                this.data.contactList.RemoveAll((m) => m.gameObject == other.gameObject);

                OnGroundTest();
            }

            private void OnGroundTest()
            {
                data.onGround = data.contactList.Exists((x) => x.normal.Angle(Vector2.up) < 5);
                if (data.onGround)
                {
                    if (state != null)
                    {
                        state.Add(AllState.OnGround);

                    }

                }
                else
                {
                    if (state != null)
                    {
                        state.Remove(AllState.OnGround);
                    }
                }
            }





            [System.Serializable]
            public class ContactObject
            {
                public GameObject gameObject;
                public Vector2 normal;

            }


        }
    }
}
