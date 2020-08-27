using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_FixedTo : MonoBehaviour {
    public GameObject[] fixedTargets = new GameObject[0];
    public Rigidbody2D objectToFixed = null;
    public Collider2D findTargetZone = null;
    public Collider2D releaseClickZone = null;
    [SerializeField, ReadOnly] private GameObject targetObject = null;
    [SerializeField, ReadOnly] private GameObject pointEventObj = null;
    [SerializeField, ReadOnly] private FixedJoint2D fixedJointComp = null;

    private void Awake () {
        findTargetZone.Ex_AddTriggerEvent (
            enter: (cl) => {
                if (fixedTargets.Contain (cl.gameObject)) {
                    targetObject = cl.gameObject;
                    pointEventObj = this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d) => {
                        if (targetObject) {
                            SetUpFixe ();
                        }
                    });

                }

            },
            exit: (cl) => {
                if (targetObject == cl.gameObject) {
                    targetObject = null;
                }
                if (pointEventObj) {
                    Destroy (pointEventObj.gameObject);
                }
            });
    }


    private void Start () {

    }

    private void SetUpFixe () {
        fixedJointComp = gameObject.AddComponent<FixedJoint2D> ();
        fixedJointComp.connectedBody = targetObject.GetComponent<Rigidbody2D> ();
        fixedJointComp.autoConfigureConnectedAnchor = false;
        fixedJointComp.connectedAnchor = Vector2.zero;

        if (releaseClickZone) {
            this.Ex_AddInputEventToTriggerOnece (releaseClickZone, EventTriggerType.PointerClick, (d) => {
                Destroy (fixedJointComp);
            });
        }

    }
}