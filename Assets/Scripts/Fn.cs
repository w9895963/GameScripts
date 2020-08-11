using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public static class Fn {

    public static void DrawCross (Vector2 position, float l = 0.2f) {

        Vector2 p = position;
        Vector2 p2 = position;
        Vector2 p3 = position;
        Vector2 p4 = position;
        p.x -= l;
        p2.x += l;
        p3.y -= l;
        p4.y += l;
        Debug.DrawLine (p, p2, Color.red, 1f);
        Debug.DrawLine (p3, p4, Color.red, 1f);
    }

    public static void DrawVector (Vector2 position, Vector2 vector, float time = 1f) {
        Vector2 p2 = position + vector;

        Debug.DrawLine (position, p2, Color.red, time);
        //Debug.DrawLine (position, p2, Color.red);
        DrawCross (p2);
    }

    public static GameObject Create (GameObject obj, Vector2 position, float rotate = 0) {
        return GameObject.Instantiate (obj, position, Quaternion.AngleAxis (rotate, Vector3.forward));
    }




    public static Vector2 RotateClock (Vector2 vector, float angle) {
        return Quaternion.AngleAxis (angle, Vector3.back) * vector;
    }


    public static GameObject[] ArrayAddUniq (GameObject[] list, GameObject obj) {
        List<GameObject> l = new List<GameObject> (list);
        if (!l.Contains (obj)) l.Add (obj);
        return l.ToArray ();
    }
    public static GameObject[] ArrayRemove (GameObject[] list, GameObject obj) {
        List<GameObject> l = new List<GameObject> (list);
        l.Remove (obj);
        return l.ToArray ();
    }


    public static void WaitToCall (float time, UnityAction call) {
        GameObject obj = new GameObject ();
        Fn_Timer timer = obj.AddComponent<Fn_Timer> ();
        timer.WaitToCall (time, call);
    }

    public static EventTrigger.Entry AddEventToTrigger (GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger eventTrigger = obj.GetComponent<EventTrigger> ();
        EventTrigger trigger = eventTrigger != null ? eventTrigger : obj.AddComponent<EventTrigger> ();
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = type;
        entry.callback.AddListener (action);
        trigger.triggers.Add (entry);
        return entry;
    }
    public static void RemoveEventToTrigger (GameObject obj, EventTriggerType type, EventTrigger.Entry entry) {
        EventTrigger eventTrigger = obj.GetComponent<EventTrigger> ();
        EventTrigger trigger = eventTrigger != null ? eventTrigger : obj.AddComponent<EventTrigger> ();
  
        trigger.triggers.Remove (entry);
    }

    public static void AddOneTimeListener (UnityEvent[] eventList, UnityAction atn) {
        UnityAction action = null;
        action = () => {
            atn ();
            foreach (var e in eventList)
                e.RemoveListener (action);
        };
        foreach (var e in eventList)
            e.AddListener (action);

    }
    public static void AddOneTimeListener (UnityEvent singleEvent, UnityAction atn) {
        UnityAction action = null;
        action = () => {
            atn ();
            singleEvent.RemoveListener (action);
        };
        singleEvent.AddListener (action);

    }
}