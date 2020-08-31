using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Placing : IC_Base {
    public bool autoQuit = true;
    public Rigidbody2D targetBody;
    public Events events = new Events ();
    public List<Object> elist = new List<Object> (0);
    [SerializeField, ReadOnly] private Vector2 targetPosition;



    void Update () {
        Fn._.DrawLineOnScreen (Gb.MainCharactor.transform.position, targetPosition, 0.01f);
    }
    public override void EnableAction () {
        events.inEvent.Invoke ();
        Gb.CanvasTopLayer.enabled = true;



        var e = Fn._.AddPointerEvent (PointerEventType.onMove, (d) => {
            targetPosition = d.position_Screen.ScreenToWold ();
        });

        elist.Add (0, e);

        var e1 = Fn._.AddPointerEvent (PointerEventType.onClick, (d) => {
            enabled = false;
        });
        elist.Add (1, e1);


    }
    public override void DisableAction () {
        elist[0].Destroy ();
        Gb.CanvasTopLayer.enabled = false;




        events.outEvent.Invoke ();
    }


    //* Class Definition

    [System.Serializable]
    public class Events {
        public UnityEvent outEvent = new UnityEvent ();
        public UnityEvent inEvent = new UnityEvent ();
    };
}