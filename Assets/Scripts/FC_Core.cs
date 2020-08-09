using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;

public class FC_Core : MonoBehaviour {
    [Header ("Requre")]
    public Rigidbody2D targetRigidbody;
    public bool autoCalculateMass = true;



    private Vector2 velocity;

    // public List;
    public List<ForceModifier> modifierList = new List<ForceModifier> ();




    private void OnEnable () {
        Sort ();
    }



    private void FixedUpdate () {
        velocity = targetRigidbody.velocity;

        Vector2 forceAdd = Vector2.zero;


        for (int i = 0; i < modifierList.Count; i++) {
            modifierList[i].act (modifierList[i]);
        }



        foreach (var e in modifierList) {
            forceAdd += e.forceAdd;

        }


        if (autoCalculateMass) forceAdd *= targetRigidbody.mass;

        targetRigidbody.AddForce (forceAdd);


        foreach (var e in modifierList) {
            e.forceAdd = Vector2.zero;
        }

    }




    //*Public Method




    public void AddModifier (UnityAction<ForceModifier> calcForce, int runOrder = 0) {

        modifierList.Add (new ForceModifier (runOrder, this, calcForce));

    }


    public void Sort () {
        modifierList.Sort ((am, bm) => {

            if (am.order * bm.order >= 0) {
                return (int) Mathf.Sign (am.order - bm.order);
            } else {
                return am.order < 0 ? 1 : -1;
            }

        });
    }
    public Vector2 VelosityPredict () {

        Vector2 forceAdd = Vector2.zero;


        foreach (var e in modifierList) {
            forceAdd += e.forceAdd;
        }

        float mass = autoCalculateMass?1 : targetRigidbody.mass;
        return targetRigidbody.velocity + forceAdd * Time.fixedDeltaTime / mass;

    }




    public class ForceModifier {
        public int order = 0;
        public UnityAction<ForceModifier> act;
        public Vector2 forceAdd;
        public FC_Core core;


        public ForceModifier (int order_, FC_Core forceCore_, UnityAction<ForceModifier> action_) {
            order = order_;
            act = action_;
            core = forceCore_;
        }
    }



    private void OnValidate () {
        if (!targetRigidbody)
            targetRigidbody = GetComponent<Rigidbody2D> ();
    }
}