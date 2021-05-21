using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScale : MonoBehaviour
{
    public bool enableUpdateObjectDate = false;
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

            Vector2 p = da.position.ScreenToWold();
            Vector2 scaleValue = p - gameObject.GetPosition2d();
            gameObject.SetScale(scaleValue);
            UpdateObjectDate();

        });
        et.triggers.Add(dragTrigger);



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
            ObjectDate.UpdateData(gameObject, ObjectDateType.Scale2D, gameObject.GetScale2d());
        }
    }
}
