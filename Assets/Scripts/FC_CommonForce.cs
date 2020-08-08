﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static FC_Core;

public class FC_CommonForce : MonoBehaviour {
    // public EventTrigger;
    public FC_Core core;
    public Gravity gravity;
    public Force force;
    public ForceToPoint forceToPoint;
    public Decelerate decelerate;
    public MaxSpeed maxSpeed;

    public DragToForce dragToForce;

    private void Awake () {
        core.modifierList.Add (new ForceModifier (0, core, gravity.CalcForce));
        core.modifierList.Add (new ForceModifier (1, core, force.CalcForce));
        core.modifierList.Add (new ForceModifier (1, core, forceToPoint.CalcForce));
        core.modifierList.Add (new ForceModifier (2, core, decelerate.CalcForce));
        core.modifierList.Add (new ForceModifier (2, core, maxSpeed.CalcForce));

        dragToForce.ApplyEvent (gameObject, force, forceToPoint);
    }

    [System.Serializable]
    public class Gravity {
        public bool enable = true;
        public M_Gravity getGravityFrom;
        public Vector2 gravityValue = new Vector2 (0, -60);

        public void CalcForce (ForceModifier mod) {

            if (enable) {


                if (getGravityFrom) {
                    gravityValue = getGravityFrom.GetGravity ();
                }


                mod.forceAdd = gravityValue;
            }

        }
    }

    [System.Serializable]
    public class Force {
        public bool enable = true;
        public Vector2 force;

        public void CalcForce (ForceModifier mod) {

            if (enable) {




                mod.forceAdd = force;
            }

        }
    }

    [System.Serializable]
    public class ForceToPoint {
        public bool enable = true;
        public Vector2 force;
        public Vector3 deltaP;

        public void CalcForce (ForceModifier mod) {

            if (enable) {
                Vector2 vector3 = mod.core.GetComponent<Transform> ().transform.TransformDirection (deltaP);
                Vector2 p = mod.core.GetComponent<Rigidbody2D> ().position + vector3;
                Fn.DrawVector (p, force.normalized);

                mod.core.GetComponent<Rigidbody2D> ().AddForceAtPosition (force, p);
            }

        }
    }

    [System.Serializable]
    public class Decelerate {
        public bool enable = true;
        public M_Gravity getGravityFrom;
        public Rigidbody2D GetRigidbody;
        public float decelerateH = 150f;
        public float decelIncRateH = 10f;
        public float decelerateV = 0;
        public float decelIncRateV = 3;
        private Vector2 velosityPredict;
        private Vector2 addH;
        private Vector2 addV;

        public void CalcForce (ForceModifier mod) {

            if (enable) {
                Vector2 gravity = getGravityFrom.GetGravity ();
                velosityPredict = mod.core.VelosityPredict ();
                Vector2 vOn_H_P = Vector3.ProjectOnPlane (velosityPredict, gravity);
                Vector2 vOn_V_P = velosityPredict - vOn_H_P;
                Vector2 v_H_C = Vector3.ProjectOnPlane (mod.core.targetRigidbody.velocity, gravity);
                Vector2 v_V_C = mod.core.targetRigidbody.velocity - v_H_C;

                float nextDV_H = decelerateH + decelIncRateH * (v_H_C.magnitude);
                float mass = mod.core.autoCalculateMass?1 : GetRigidbody.mass;

                bool DelIsBiggerThanSpeedH = nextDV_H / mass * Time.fixedDeltaTime > vOn_H_P.magnitude;
                if (DelIsBiggerThanSpeedH) {
                    addH = -1 * vOn_H_P / Time.fixedDeltaTime * mass;
                } else {
                    addH = nextDV_H * vOn_H_P.normalized * -1;
                }

                float nextDV_V = decelerateV + decelIncRateV * (v_V_C.magnitude);
                bool DelIsBiggerThanSpeedV = nextDV_V / mass * Time.fixedDeltaTime > vOn_V_P.magnitude;
                if (DelIsBiggerThanSpeedV) {
                    addV = -1 * vOn_V_P / Time.fixedDeltaTime * mass;
                } else {
                    addV = nextDV_V * vOn_V_P.normalized * -1;
                }


                mod.forceAdd = addH + addV;
            }

        }
    }

    [System.Serializable]
    public class MaxSpeed {
        public bool enable = true;
        public float max = 20f;

        public void CalcForce (ForceModifier mod) {

            if (enable) {

                // Vector2 add = default;

                var rb = mod.core.targetRigidbody;
                Vector2 velocity = rb.velocity;

                if (velocity.magnitude > max) {
                    rb.velocity = velocity.normalized * max;
                }



                // mod.forceAdd = add;
            }

        }
    }

    [System.Serializable]
    public class DragToForce {
        public float multiply = 300;
        public float maxDistance = 0.2f;
        private Vector2 positionBegin;
        private Vector2 deltaP;

        public void ApplyEvent (GameObject gameObject, Force force, ForceToPoint forceToPoint) {
            Fn.AddListener (gameObject, EventTriggerType.Drag, (data) => {
                var da = (PointerEventData) data;
                Vector2 delta = da.delta;
                Vector2 p1 = Camera.main.ScreenToViewportPoint (da.position);
                Vector2 p2 = Camera.main.ScreenToViewportPoint (da.pressPosition);
                Vector2 vector = p2 - p1;
                vector = Mathf.Clamp (vector.magnitude, 0, maxDistance) * vector.normalized;
                // vector.x = Mathf.Clamp (vector.x, -maxDistance, maxDistance);
                // vector.y = Mathf.Clamp (vector.y, -maxDistance, maxDistance);

                Vector2 v2 = vector.magnitude * multiply * -vector.normalized;
                forceToPoint.force = v2;

                //  force.force = v2;


                Vector2 vector3 = gameObject.transform.TransformDirection (deltaP);
                Vector2 p = gameObject.GetComponent<Rigidbody2D> ().position + vector3;
                //Fn.DrawVector (p, v2);

                //gameObject.GetComponent<Rigidbody2D> ().AddForceAtPosition (v2, p);

            });
            Fn.AddListener (gameObject, EventTriggerType.EndDrag, (data) => {
                force.force = Vector2.zero;
                forceToPoint.force = default;
            });
            Fn.AddListener (gameObject, EventTriggerType.BeginDrag, (data) => {
                var da = (PointerEventData) data;
                positionBegin = gameObject.GetComponent<Rigidbody2D> ().position;
                Vector2 p2_W = Camera.main.ScreenToWorldPoint (da.pressPosition);

                forceToPoint.deltaP = gameObject.transform.InverseTransformDirection (p2_W - positionBegin);
            });



        }
    }
}