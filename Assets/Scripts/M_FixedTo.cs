using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class M_FixedTo : MonoBehaviour {
    public GameObject[] fixedTargets = new GameObject[0];
    public Rigidbody2D objectToFixed = null;
    public Collider2D findTargetZone = null;
    public Vector2 fixedAnchor = default;
    public bool enableClickToSetFixe = false;
    public Realise realiseSet = new Realise ();


    [SerializeField, ReadOnly] private GameObject targetObject = null;
    [SerializeField, ReadOnly] private FixedJoint2D fixedJointComp = null;
    [SerializeField] private List<Object> elist = new List<Object> ();


    private void OnEnable () {
        Setup ();
    }
    private void OnDisable () {

        Fn._.Destroy (elist.ToArray ());
        fixedJointComp.Destroy ();
    }

    private void Setup () {
        elist.Add (4, default);
        var ev = findTargetZone.Ex_AddCollierEvent (
            onTriggerEnter: (cl) => {
                if (fixedTargets.Contain (cl.gameObject)) {
                    targetObject = cl.gameObject;

                    if (enableClickToSetFixe) {
                        elist[1] = this.Ex_AddPointerEventOnece (PointerEventType.onClick, (d) => {
                            if (targetObject) {
                                SetUpFixe ();
                            }
                        });
                    }

                }

            },
            OnTriggerExit: (cl) => {
                if (targetObject == cl.gameObject) {
                    targetObject = null;
                }
                elist[1].Destroy ();
            });

        elist.Add (0, ev);
    }


    private void DragToRealiseSetup (bool enabled) {
        if (enabled) {
            if (realiseSet.mode == RealiseMode.drag) {
                var ev = this.Ex_AddInputToTriggerOnece (realiseSet.releaseClickZone.gameObject, EventTriggerType.Drag,
                    (d) => {
                        M_Pickable comp = GetComponent<M_Pickable> ();
                        if (comp) {
                            comp.state.ChangeState (M_Pickable.State.inhand);
                            Destroy (fixedJointComp);
                        }
                    });
                elist[3] = ev;
            }
        } else {
            elist[3].Destroy ();
        }
    }



    private void SetUpFixe () {
        fixedJointComp = gameObject.AddComponent<FixedJoint2D> ();
        fixedJointComp.connectedBody = targetObject.GetComponent<Rigidbody2D> ();
        fixedJointComp.autoConfigureConnectedAnchor = false;
        fixedJointComp.connectedAnchor = Vector2.zero;
        fixedJointComp.anchor = fixedAnchor;



        DragToRealiseSetup (true);

    }




    //*Property
    [System.Serializable]
    public class Realise {
        public RealiseMode mode = RealiseMode.diable;
        public Collider2D releaseClickZone = null;




    }
    public enum RealiseMode {
        diable,
        drag,
        click
    }

    //*Public

    public bool TryToConnect () {
        if (targetObject) {
            SetUpFixe ();
        }
        return targetObject != null;
    }



}