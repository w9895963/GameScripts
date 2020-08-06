using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FC_Core;

public class FC_Player : MonoBehaviour {
    public FC_Core core;
    public Gravity gravity;
    public HorizontalMove horizontalMove;
    public DecelerateTwoAxis decelerate;



    private void Awake () {
        core.modifierList.Add (new ForceModifier (0, GetComponent<FC_Core> (), gravity.CalcForce));
        core.modifierList.Add (new ForceModifier (1, GetComponent<FC_Core> (), horizontalMove.ClacForce));
        core.modifierList.Add (new ForceModifier (2, GetComponent<FC_Core> (), decelerate.ClacForce));
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
    public class DecelerateTwoAxis {
        public bool enable = true;
        public M_Gravity getGravityFrom;
        public Rigidbody2D GetRigidbody;
        public float decelerate = 150f;
        public float decelIncRate = 10f;
        public float decelerateV = 150f;
        public float decelIncRateV = 10f;
        public Vector2 velosityPredict;
        private Vector2 addH;
        private Vector2 addV;

        public void ClacForce (ForceModifier mod) {

            if (enable) {
                Vector2 gravity = getGravityFrom.GetGravity ();
                velosityPredict = mod.core.VelosityPredict ();
                Vector2 vOn_H = Vector3.ProjectOnPlane (velosityPredict, gravity);
                Vector2 vOn_V = velosityPredict - vOn_H;


                bool DelIsBiggerThanSpeedH = decelerate / GetRigidbody.mass * Time.fixedDeltaTime > vOn_H.magnitude;
                if (DelIsBiggerThanSpeedH) {
                    addH = -1 * vOn_H / Time.fixedDeltaTime * GetRigidbody.mass;
                } else {
                    float mat = decelerate + decelIncRate * vOn_H.magnitude;
                    addH = mat * vOn_H.normalized * -1;
                }


                bool DelIsBiggerThanSpeedV = decelerateV / GetRigidbody.mass * Time.fixedDeltaTime > vOn_V.magnitude;
                if (DelIsBiggerThanSpeedV) {
                    addV = -1 * vOn_V / Time.fixedDeltaTime * GetRigidbody.mass;
                } else {
                    float f = decelerateV + decelIncRateV * vOn_V.magnitude;
                    addV = f * vOn_V.normalized * -1;
                }


                mod.forceAdd = addH + addV;
            }

        }
    }
}