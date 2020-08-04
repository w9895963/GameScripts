using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_TriggerDead : MonoBehaviour {
    public UnityEvent onEnter;
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "CurrentPlayer") {
            Time.timeScale = 0.05f;
            onEnter.Invoke ();
            Fn.WaitToCall (0.07f, () => {
                other.gameObject.GetComponent<M_Reset> ().Reset ();
            });
            Fn.WaitToCall (0.1f, () => { Time.timeScale = 1; });
        }
    }


    public void DeadProcess (Callback callback) {


    }


    public delegate void Callback ();

}