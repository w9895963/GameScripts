using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Placing : MonoBehaviour {
    public bool autoQuit = true;
    public Rigidbody2D targetBody;
    public Events events = new Events ();
    public List<Object> elist = new List<Object> (0);
    [SerializeField, ReadOnly] private Vector2 targetPosition;



    void Update () {
        Fn._.DrawLineOnScreen (Gb.MainCharactor.transform.position, targetPosition, 0.01f);
    }
    private void OnEnable () {
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


    private void OnDisable () {
        elist[0].Destroy ();
        Gb.CanvasTopLayer.enabled = false;


        if (autoQuit) {
            var c = GetComponent<I_Connecter> ();
            c.Enable ();
        }


        events.outEvent.Invoke ();
    }


    //*PUlic
    public void Enable () => enabled = true;
    public void Disable () => enabled = false;

    [System.Serializable]
    public class Events {
        public UnityEvent outEvent = new UnityEvent ();
        public UnityEvent inEvent = new UnityEvent ();
    };
}