using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinder : MonoBehaviour
{
    public enum Date { GroundNormal }
    public enum State { OnGround }
    public Main main = new Main();
    private void Awake()
    {
        main.gameObject = gameObject;
    }
    private void OnEnable()
    {
        main.Enable();
    }
    private void OnDisable()
    {
        main.Disable();
    }

    [System.Serializable]
    public class Main
    {
        public Vector2 defaultGroundNormal = Vector2.up;
        public float groundNormalAngleAllow = 5f;
        public GameObject gameObject;


        public Vector2 groundNormal;
        public bool isOnGround = false;
        public List<ContactObject> contactList = new List<ContactObject>();



        private void OnCollisionEnter2DAction(Collision2D other)
        {
            ContactObject contactObj = new ContactObject();
            contactObj.gameObject = other.gameObject;
            Vector2 normal = other.contacts[0].normal;
            contactObj.normal = normal;
            contactList.Add(contactObj);

            OnGroundTest();
        }
        private void OnCollisionExit2DAction(Collision2D other)
        {
            contactList.RemoveAll((m) => m.gameObject == other.gameObject);

            OnGroundTest();
        }

        private void OnGroundTest()
        {
            var baseNormal = this.defaultGroundNormal;
            bool lastOnGround = isOnGround;
            ContactObject contact = contactList.Find((x) =>
            {
                return x.normal.Angle(baseNormal) < groundNormalAngleAllow;
            });

            bool currOnGround = contact != null;

            if (currOnGround != lastOnGround)
            {
                isOnGround = currOnGround;
            }
            groundNormal = contact?.normal ?? Vector2.zero;

            DataUPdate();
            StateUpdate();

        }



        private void StateUpdate()
        {
            if (isOnGround)
            {
                ObjectState.State.Add(gameObject, State.OnGround);
            }
            else
            {
                ObjectState.State.Remove(gameObject, State.OnGround);
            }

        }

        private void DataUPdate()
        {
            ObjectDate.UpdateData(gameObject, Date.GroundNormal, groundNormal);
        }




        public void Enable()
        {
            Disable();
            BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2DAction);
            BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2DAction);
        }
        public void Disable()
        {
            BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2DAction);
            BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2DAction);
        }


        [System.Serializable]
        public class ContactObject
        {
            public GameObject gameObject;
            public Vector2 normal;

        }

    }
}
