﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_OrderRun {
    private static List<UnityAction> actions = new List<UnityAction> ();


    public static void OrderRun (UnityAction action) {
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



public static class _Extension_C_OrderRun {
    public static void OrderRun (this Fn fn, UnityAction action) {
        C_OrderRun.OrderRun (action);
    }
}