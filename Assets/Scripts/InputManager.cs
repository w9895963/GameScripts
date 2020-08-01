using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using static Fn;

public class InputManager : MonoBehaviour {
    public bool setMovingPoint;
    public Vector2 movingPoint;


    public UnityEvent<Vector2> setDestination;
    public UnityEvent onSetDestination;


    public GameObject currentPlayer () => GameObject.FindGameObjectWithTag ("CurrentPlayer");


    public void SetMovingPoint () {


        if (setMovingPoint) {

            Vector3 ClickPosition = Camera.main.ScreenToWorldPoint (Pointer.current.position.ReadValue ());
            setDestination.Invoke (ClickPosition);
            onSetDestination.Invoke ();


        }


    }


}