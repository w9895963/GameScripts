using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Connecter : MonoBehaviour {
    public Rigidbody2D rigidToConnect;
    public ConnectMethod connectMethod = ConnectMethod.Instance;
    public List<GameObject> targets = new List<GameObject> (1);
    public float allowDistance = 1f;
    public Vector2 fixedAnchor = Vector2.zero;
    public bool autoExit = true;




    public BreakMethod breakMethod = new BreakMethod ();
    public Events events = new Events ();
    [ReadOnly] public FixedJoint2D fixedJointComp;
    [ReadOnly] public Rigidbody2D targetBody;
    public List<Object> elist = new List<Object> (1);
    [SerializeField, ReadOnly] private bool conected = false;



    private void OnEnable () {
        events.enter.Invoke ();
        if (connectMethod == ConnectMethod.Instance) {
            Rigidbody2D closedBody;
            bool distanceAllow;
            DistanceTest (out closedBody, out distanceAllow);

            if (distanceAllow) {
                targetBody = closedBody;
                conected = true;
                Connect ();
            } else {
                conected = false;
                Disable ();
            }
        }


        if (connectMethod == ConnectMethod.Lazy) {
            LazyConectSetup (true);

        }

    }



    private void OnDisable () {
        fixedJointComp.Destroy ();
        BreakConectSetup (false);
        LazyConectSetup (false);



        if (autoExit) {
            if (!conected) {
                if (GetComponent<I_Grab> ())
                    GetComponent<I_Grab> ().enabled = true;
            } else {
                var c = GetComponent<I_Input> ();
                if (c) c.enabled = true;
            }
        }

        events.exitEvent.Invoke ();
    }


    //*Private
    private void DistanceTest (out Rigidbody2D closedBody, out bool distanceAllow) {
        Vector2 p = rigidToConnect.position;
        targets.Sort ((a, b) => {
            return (a.Get2dPosition () - p).magnitude < (b.Get2dPosition () - p).magnitude ? -1 : 1;
        });
        closedBody = targets[0].GetComponent<Rigidbody2D> ();
        float closedDist = (closedBody.position - p).magnitude;
        distanceAllow = closedDist <= allowDistance;
    }
    private void Connect () {
        fixedJointComp = rigidToConnect.gameObject.AddComponent<FixedJoint2D> ();
        fixedJointComp.connectedBody = targetBody;
        fixedJointComp.autoConfigureConnectedAnchor = false;
        fixedJointComp.connectedAnchor = Vector2.zero;
        fixedJointComp.anchor = fixedAnchor;

        BreakConectSetup (true);

    }
    private void LazyConectSetup (bool enabled) {
        if (enabled) {
            var e = gameObject.Ex_AddCollierEvent ((c) => {
                Debug.Log (45454);
                fixedJointComp = rigidToConnect.Ex_ConnectTo (c.attachedRigidbody);
                LazyConectSetup (false);
            }, targetFilter : targets.ToArray ());
            elist.Add (2, e);
        } else {
            elist[2].Destroy ();
        }
    }
    private void BreakConectSetup (bool enabled) {
        if (enabled) {
            if (breakMethod.click) {
                var e = breakMethod.clickBox.Ex_AddInputToTriggerOnece (EventTriggerType.PointerClick, (d) => {
                    Disable ();
                });
                elist.Add (0, e);
            }
            if (breakMethod.drag) {
                var e = breakMethod.clickBox.Ex_AddInputToTriggerOnece (EventTriggerType.Drag, (d) => {
                    Disable ();
                });
                elist.Add (1, e);
            }
        } else {
            elist[0].Destroy ();
            elist[1].Destroy ();
        }
    }


    //* Public Method
    public void Enable () => enabled = true;
    public void Disable () => enabled = false;

    //* Class Definition
    [System.Serializable]
    public class BreakMethod {
        public bool click = false;
        public bool drag = false;
        public Collider2D clickBox;

    }


    public enum ConnectMethod { Instance, Lazy }

    [System.Serializable]
    public class Events {
        public UnityEvent exitEvent = new UnityEvent ();
        public UnityEvent enter = new UnityEvent ();
    };
}