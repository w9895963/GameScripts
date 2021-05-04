using System.Collections;
using System.Collections.Generic;
using CharacterBundle;
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
                BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter);
                BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionEnter);
            }




            private void OnCollisionEnter(Collision2D other)
            {
                ContactObject contactObj = new ContactObject();
                contactObj.gameObject = other.gameObject;
                Vector2 normal = other.contacts[0].normal;
                contactObj.normal = normal;
                this.data.contactList.Add(contactObj);

                OnGroundTest();

            }

            private void OnCollisionExit(Collision2D other)
            {
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
                        state.Add(CharacterState.OnGround);

                    }

                }
                else
                {
                    if (state != null)
                    {
                        state.Remove(CharacterState.OnGround);
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
