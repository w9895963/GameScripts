using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;


public class GroundTester : MonoBehaviour
{
    #region class
    [System.Serializable]
    public class ContactObject
    {
        public GameObject gameObject;
        public Vector2 normal;

    }
    [System.Serializable]
    public class Preference
    {
        public GameObject gameObject;
        public Vector2 groundNormal = Vector2.up;
        public float goundNormalAngleAllow = 5;
    }
    [System.Serializable]
    public class Datas
    {
        public List<ContactObject> contactList = new List<ContactObject>();
        public bool onGround = false;
    }
    #endregion


    #region Fields


    private Preference pref;
    private Vector2 groundNormal => pref.groundNormal;
    private float goundNormalAngleAllow => pref.goundNormalAngleAllow;

    private Datas data = new Datas();
    public Datas Data => data;
    private List<ContactObject> contactList { set => data.contactList = value; get => data.contactList; }
    private bool onGround { set => data.onGround = value; get => data.onGround; }
    #endregion


    public GroundTester(Preference pref)
    {
        this.pref = pref;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactObject contactObj = new ContactObject();
        contactObj.gameObject = other.gameObject;
        Vector2 normal = other.contacts[0].normal;
        contactObj.normal = normal;
        contactList.Add(contactObj);

        OnGroundTest();
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        contactList.RemoveAll((m) => m.gameObject == other.gameObject);

        OnGroundTest();
    }



    private void OnGroundTest()
    {
        var groundNormal = this.groundNormal;
        onGround = contactList.Exists((x) =>
        {
            return x.normal.Angle(groundNormal) < goundNormalAngleAllow;
        });
        if (onGround)
        {


        }
        else
        {

        }
    }
    public void Enable()
    {

    }
    public void Disable()
    {

    }

}

