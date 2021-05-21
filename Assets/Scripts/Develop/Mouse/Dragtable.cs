using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragtable : MonoBehaviour
{
    public bool enableUpdateObjectDate = false;
    public Vector2 startPosition;
    public Vector2 startObjPosition;
    public UnityEvent OnDragFinished = new UnityEvent();
    void Start()
    {
        Collider2D com = gameObject.GetComponent<Collider2D>();
        if (com == null) { gameObject.AddComponent<PolygonCollider2D>(); }

        EventTrigger et = gameObject.GetComponent<EventTrigger>(true);


        EventTrigger.Entry dragTrigger = new EventTrigger.Entry();
        dragTrigger.eventID = EventTriggerType.Drag;
        dragTrigger.callback.AddListener((d) =>
        {
            PointerEventData da = d as PointerEventData;

            Vector2 deltaP = da.position.ScreenToWold() - startPosition;
            gameObject.SetPosition(startObjPosition + deltaP);
            UpdateObjectDate();
        });
        et.triggers.Add(dragTrigger);


        EventTrigger.Entry downTrigger = new EventTrigger.Entry();
        downTrigger.eventID = EventTriggerType.PointerDown;
        downTrigger.callback.AddListener((d) =>
        {
            PointerEventData da = d as PointerEventData;
            startPosition = da.position.ScreenToWold();
            startObjPosition = gameObject.GetPosition2d();
        });
        et.triggers.Add(downTrigger);


        EventTrigger.Entry upTrigger = new EventTrigger.Entry();
        upTrigger.eventID = EventTriggerType.EndDrag;
        upTrigger.callback.AddListener((d) =>
        {
            OnDragFinished.Invoke();
        });
        et.triggers.Add(upTrigger);



    }

    private void UpdateObjectDate()
    {
        if (enableUpdateObjectDate)
        {
            ObjectDate.UpdateData(gameObject, ObjectDateType.Position2D, gameObject.GetPosition2d());
        }
    }
}
