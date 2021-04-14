using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrderRun {
    private static List<UnityAction> actions = new List<UnityAction> ();


    public static void AddAction (UnityAction action) {
        actions.Add (() => {
            action ();
            actions.RemoveAt (0);
            if (actions.Count > 0) {
                actions[0].Invoke ();
            }
        });
        if (actions.Count == 1) {
            actions[0].Invoke ();
        }
    }
}



public static class Extension_C_OrderRun {
    public static void OrderRun (this Global.Function fn, UnityAction action) {
        global::OrderRun.AddAction (action);
    }
}