using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BasicEvent
{
  

    public class OnPointerDrag
    {
        static PrefabBundle.Prefab backGroundPrefab = PrefabI.BackgroundUI;

        public class DragDate
        {
            public Vector2 beginScreenPosition;
            public Vector2 screenPosition;
            public Vector2 screenDelta;
            public Vector2 position => screenPosition.ScreenToWold();
            public Vector2 beginPosition => beginScreenPosition.ScreenToWold();
            public EventTrigger.Entry onStart;
            public EventTrigger.Entry onDrag;
            public EventTrigger.Entry onEnd;

        }





        public static void Add(GameObject gameObject, Action<DragDate> onDrag = null, Action<DragDate> onEnd = null, Action<DragDate> onStart = null)
        {
            Collider2D com = gameObject.GetComponent<Collider2D>();
            if (com == null) { gameObject.AddComponent<PolygonCollider2D>(); }
            EventTrigger et = gameObject.GetComponent<EventTrigger>(true);

            DragDate date = new DragDate();
            DateF.AddDate(gameObject, date);


            EventTrigger.Entry dragBeginTrigger = new EventTrigger.Entry();
            dragBeginTrigger.eventID = EventTriggerType.BeginDrag;
            dragBeginTrigger.callback.AddListener((d) =>
            {
                PointerEventData da = d as PointerEventData;

                DragDate date = DateF.GetDate<DragDate>(gameObject);

                date.screenPosition = da.position;
                date.beginScreenPosition = da.position - da.delta;
                date.screenDelta = da.delta;

                DateF.AddDate(gameObject, date);
                onStart?.Invoke(date);

            });
            et.triggers.Add(dragBeginTrigger);
            date.onStart = dragBeginTrigger;



            EventTrigger.Entry dragTrigger = new EventTrigger.Entry();
            dragTrigger.eventID = EventTriggerType.Drag;
            dragTrigger.callback.AddListener((d) =>
            {
                PointerEventData da = d as PointerEventData;
                DragDate dragDate = DateF.GetDate<DragDate>(gameObject);
                dragDate.screenPosition = da.position;
                dragDate.screenDelta = da.delta;
                onDrag?.Invoke(dragDate);

            });
            et.triggers.Add(dragTrigger);
            date.onDrag = dragTrigger;




            EventTrigger.Entry upTrigger = new EventTrigger.Entry();
            upTrigger.eventID = EventTriggerType.EndDrag;
            upTrigger.callback.AddListener((d) =>
            {
                PointerEventData da = d as PointerEventData;
                DragDate dragDate = DateF.GetDate<DragDate>(gameObject);
                dragDate.screenPosition = da.position;
                onEnd?.Invoke(dragDate);
            });
            et.triggers.Add(upTrigger);
            date.onEnd = upTrigger;

        }
        public static void EmptyDrag(Action<DragDate> onDrag = null, Action<DragDate> onEnd = null, Action<DragDate> onStart = null)
        {

            var gameObject = PrefabF.FindOrCretePrefab(backGroundPrefab);

            gameObject.GetComponentInParent<Canvas>().worldCamera = Camera.main;

            Add(gameObject, onDrag, onEnd, onStart);
        }
        public static void Remove(GameObject gameObject, Action<Collider2D> action)
        {

        }

    }


}
