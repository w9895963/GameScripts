using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_Gravity : MonoBehaviour {

    public Vector2 gravity = new Vector2 (0, -60);

    public Events events;
    public Test test;


    private void Start () { }


    //* Main Function

    public void SetGravityDirection (Vector2 direction) {
        if (enabled) {
            if (gravity.normalized != direction) {

                gravity = direction.normalized * gravity.magnitude;
                events.gravityChanged.Invoke ();

            }
        }

    }
    public void SetGravityDirection (GravityDirection dir) {
        if (enabled) {
            Vector2 direction = default;
            switch (dir) {
                case GravityDirection.Up:
                    direction = Vector2.up;
                    break;
                case GravityDirection.Down:
                    direction = Vector2.down;
                    break;
                case GravityDirection.Left:
                    direction = Vector2.left;
                    break;
                case GravityDirection.Right:
                    direction = Vector2.right;
                    break;
            }


            if (gravity.normalized != direction) {

                gravity = direction.normalized * gravity.magnitude;
                events.gravityChanged.Invoke ();

            }
        }

    }
    public Vector2 GetGravity () => gravity;

    //*
    public enum GravityDirection {
        Down,
        Up,
        Left,
        Right
    }

    [System.Serializable]
    public class Events {
        public UnityEvent gravityChanged;

    }


    //* Test
    [System.Serializable]
    public class Test {
        public GravityDirection setG;


    }


    private void OnValidate () {
        Vector2[] g = { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

        SetGravityDirection (test.setG);

    }


}