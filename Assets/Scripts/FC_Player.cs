using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static FC_Core;

public class FC_Player : MonoBehaviour {
    public FC_Core core;
    public Gravity gravity;
    public HorizontalMove horizontalMove;
    public Decelerate decelerate;
    public MoveWithGround moveWithGround;



    private void Awake () {
        core.modifierList.Add (new ForceModifier (0, GetComponent<FC_Core> (), gravity.CalcForce));
        core.modifierList.Add (new ForceModifier (1, GetComponent<FC_Core> (), horizontalMove.ClacForce));
        core.modifierList.Add (new ForceModifier (2, GetComponent<FC_Core> (), decelerate.CalcForce));
        core.modifierList.Add (new ForceModifier (3, GetComponent<FC_Core> (), moveWithGround.CalcForce));
    }


    public void Walk (int direction) {
        horizontalMove.walkDirection = direction;
    }
    public void Stop () {
        horizontalMove.walkDirection = 0;
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
    public class HorizontalMove {
        public bool enable = true;
        public M_GroundFinder groundFinder;
        public M_Gravity getGravityFrom;
        [Range (-1, 1)]
        public int walkDirection;
        public float moveForce = 130;
        public float moveForceOnAir = 60;

        public void ClacForce (ForceModifier mod) {

            if (enable) {

                Vector2 normal = groundFinder.GetGroundNormal ();
                float f = moveForce;

                if (normal == default) {
                    normal = -getGravityFrom.GetGravity ();
                    f = moveForceOnAir;
                }

                Vector2 MoveDir = Fn.RotateClock (normal, 90).normalized;
                Vector2 forceAdd = walkDirection * MoveDir * f;


                mod.forceAdd = forceAdd;
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
    public class MoveWithGround {
        public bool enable = true;
        public M_GroundFinder importGround;
        public M_Gravity getGravityFrom;
        private bool lastIsOnGround;
        private Vector2 groundPositionLast;
        private GameObject lastGround;

        public void CalcForce (ForceModifier mod) {

            if (enable) {
                GameObject ground = importGround.GetGround ();
                Rigidbody2D rb = mod.core.targetRigidbody;
                Vector2 gravity = getGravityFrom.GetGravity ();
                Vector2 positionNew = default;
                if (ground) {
                    positionNew = ground.transform.position;
                    if (!lastIsOnGround) {
                        groundPositionLast = importGround.GetGround ().transform.position;
                        lastGround = importGround.GetGround ();
                        lastIsOnGround = true;
                    }
                } else {
                    lastIsOnGround = false;
                }


                if (ground & lastIsOnGround) {
                    if (lastGround == ground) {
                        Vector2 positionDelta = positionNew - groundPositionLast;

                        RaycastHit2D[] hits = new RaycastHit2D[32];
                        ContactFilter2D contactFilter = new ContactFilter2D ();
                        contactFilter.layerMask = LayerMask.GetMask ("Ground", "InvisibleWall");
                        int count = rb.Cast (positionDelta, contactFilter, hits, positionDelta.magnitude);

                        List<RaycastHit2D> hitsList = new List<RaycastHit2D> (hits);
                        hitsList.RemoveAll (hit => hit == default);
                        hitsList.RemoveAll (hit => hit.normal == -gravity.normalized);
                        foreach (var hit in hitsList) {
                            Fn.DrawVector (hit.point, hit.normal);
                        }

                        if (hitsList.Count == 0) {
                            rb.position = (rb.position + positionDelta);
                        }
                    }


                    groundPositionLast = ground.transform.position;
                    lastGround = ground;
                }



            }

        }
    }

    private void Start () {

    }
}