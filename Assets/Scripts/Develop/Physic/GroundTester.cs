using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;


public class GroundTester : MonoBehaviour
{

    #region Fields


    public Vector2 standardNormal = Vector2.up;
    public float goundNormalAngleAllow = 5f;
    public Vector2 groundNormal;

    public bool isOnGround = false;
    public List<ContactObject> contactList = new List<ContactObject>();

    #endregion
    // * ---------------------------------- 


    private void OnEnable()
    {
        BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2DAction);
        BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2DAction);
    }
    private void OnDisable()
    {
        BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2DAction);
        BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2DAction);
    }

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
        var baseNormal = this.standardNormal;
        bool lastOnGround = isOnGround;
        ContactObject contact = contactList.Find((x) =>
        {
            return x.normal.Angle(baseNormal) < goundNormalAngleAllow;
        });

        bool currOnGround = contact != null;

        if (currOnGround != lastOnGround)
        {
            isOnGround = currOnGround;
            StateUpdate();
        }

        groundNormal = contact?.normal ?? Vector2.zero;
        DataUPdate();


    }



    private void StateUpdate()
    {
        if (isOnGround)
        {
            ObjectState.State.Add(gameObject, ObjectStateName.OnGround);
        }
        else
        {
            ObjectState.State.Remove(gameObject, ObjectStateName.OnGround);
        }

    }

    private void DataUPdate()
    {
        ObjectDate.UpdateDate(gameObject, ObjectDataName.GroundNormal, groundNormal);
    }

    #region class
    [System.Serializable]
    public class ContactObject
    {
        public GameObject gameObject;
        public Vector2 normal;

    }
    #endregion


}

