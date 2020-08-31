using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class I_Clickable : MonoBehaviour {

    public Collider2D clickBox;
    public List<Object> elist = new List<Object> ();




    private void OnEnable () {
        var ev = clickBox.Ex_AddInputToTrigger (EventTriggerType.PointerClick, (d) => {
            Exit ();
        });
        elist.Add (0, ev);
    }

    private void OnDisable () {
        elist[0].Destroy ();
    }




    //*Private
    private void Exit () {

    }

}