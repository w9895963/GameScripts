using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerManager : MonoBehaviour {
    [Header ("Dependent Component")]
    public M_PlayerMove moveComp;


    private void Reset () {
        moveComp = moveComp?moveComp : GetComponent<M_PlayerMove> ();
    }


    public void MoveTo (Vector2 position) => moveComp.MoveTo (position);
    public void Move (Vector2 direction, float speed = -1) {
        if (speed != -1) {
            moveComp.maxSpeed = speed;
        }
        moveComp.Move (direction);
    }

    public void Stop () => moveComp.Stop ();

    public Vector2 position { get => gameObject.transform.position; }

}