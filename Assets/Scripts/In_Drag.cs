using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class In_Drag : MonoBehaviour {
    public float dragSpeed = 1f;
    public float undragSpeed = 0.2f;
    public EventTrigger eventTrigger;
    public M_AnimtionContrl animationCtrol;
    private void Awake () {

        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener ((e) => {

            PointerEventData data = e as PointerEventData;
            if (data.delta.y > 0) {
                animationCtrol.PlayForward (dragSpeed);
            }

        });
        eventTrigger.triggers.Add (entry);



        EventTrigger.Entry dndDrag = new EventTrigger.Entry ();
        dndDrag.eventID = EventTriggerType.EndDrag;
        dndDrag.callback.AddListener ((e) => {

            animationCtrol.playBackward (undragSpeed);

        });
        eventTrigger.triggers.Add (dndDrag);



        EventTrigger.Entry click = new EventTrigger.Entry ();
        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener ((e) => {

            if (animationCtrol.GetPlayDegree () == 0) {
                animationCtrol.PlayForward (dragSpeed);
                Fn.WaitToCall (0.1f, () => animationCtrol.playBackward (undragSpeed));
            }

        });
        eventTrigger.triggers.Add (click);


    }
}