using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_Gravity : MonoBehaviour {

    [Header ("Call")]
    public Vector2 gravity = new Vector2 (0, -60);

    public Events events;
    public Test test;


    //* Input
    public void ChangeGravityDirection (Vector2 direction) {

        if (gravity.normalized != direction) {

            gravity = direction.normalized * gravity.magnitude;
            events.gravityChanged.Invoke ();

        }

    }

    public Vector2 GetGravity () => gravity;

    [System.Serializable]
    public class Events {
        public UnityEvent gravityChanged;

    }


    //* Test
    [System.Serializable]
    public class Test {
        public TestGravity setG;


        public enum TestGravity {
            Down,
            Up,
            Left,
            Right
        }


    }


    private void OnValidate () {
        Vector2[] g = { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

        ChangeGravityDirection (g[test.setG.GetHashCode ()]);

    }


}