using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_SimpleMove : MonoBehaviour {
    [Header ("Require")]
    public Rigidbody2D rigidBody;
    [Header ("Funtion")]
    public bool repeat;
    private bool repeatSeted = false;
    [Header ("Status")]
    public bool moving;

    public PreSetMoveAction preSetMoveAction;

    public MoveData movementData;
    public Test test;


    private void FixedUpdate () {
        if (moving) {
            float moveRate = (Time.time - movementData.timeStart) / movementData.time;
            moveRate = Mathf.Clamp01 (moveRate);
            rigidBody.position = movementData.startPosition +
                (movementData.destiation - movementData.startPosition) * moveRate;

            if (moveRate == 1) {
                moving = false;
                movementData.whenFinish.Invoke ();

            }
        }

    }

    private void Update () {
        if (repeat) {
            if (!repeatSeted) {
                MoveAsPreSetRepeat ();
                repeatSeted = true;
            }
        } else {
            repeatSeted = false;
        }
    }


    public void Moveto (Vector2 dstination, float time, UnityAction callback) {
        movementData.destiation = dstination;
        movementData.time = time;
        movementData.timeStart = Time.time;
        movementData.startPosition = rigidBody.position;

        movementData.whenFinish.AddListener (callback);

        //  movementData.WhenFinish.AddListener (() => movementData.WhenFinish.RemoveListener (callback));
        moving = true;


    }
    public void Moveto (Vector2 dstination, float time) {
        movementData.destiation = dstination;
        movementData.time = time;
        movementData.timeStart = Time.time;
        movementData.startPosition = rigidBody.position;


        //  movementData.WhenFinish.AddListener (() => movementData.WhenFinish.RemoveListener (callback));
        moving = true;


    }


    public void MoveAsPreSet () {
        Vector2 d = rigidBody.position + preSetMoveAction.direction.normalized * preSetMoveAction.distance;
        Moveto (d, preSetMoveAction.time, () => preSetMoveAction.whenFinish.Invoke ());
    }
    public void MoveAsPreSetRepeat () {
        Vector2 d = rigidBody.position + preSetMoveAction.direction.normalized * preSetMoveAction.distance;

        movementData.whenFinish.AddListener (() => {
            IEnumerator ExampleCoroutine () {
                yield return new WaitForSeconds (1);
                Moveto (movementData.startPosition, movementData.time);
            }
            StartCoroutine (ExampleCoroutine ());
            // Moveto(movementData.startPosition, movementData.time);
        });


        Moveto (d, preSetMoveAction.time);

    }

    [System.Serializable]
    public class MoveData {
        public float timeStart;
        public Vector2 startPosition;
        public Vector2 destiation;
        public float time;
        public UnityEvent whenFinish;


    }

    [System.Serializable]
    public class PreSetMoveAction {
        public Vector2 direction;
        public float distance;
        public float time;
        public UnityEvent whenFinish;
    }

    [System.Serializable]
    public class Test {
        public bool move;
    }
    private void OnValidate () {
        if (test.move) {
            test.move = false;


            MoveAsPreSet ();

        }
    }
}