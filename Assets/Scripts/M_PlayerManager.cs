using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerManager : MonoBehaviour {
    [Header ("Dependent Component")]
    public M_PlayerClickMove playerClickMove;


    private void OnValidate () {
        playerClickMove = playerClickMove?playerClickMove : GetComponent<M_PlayerClickMove> ();
        Gb.charactorMain = Gb.charactorMain?Gb.charactorMain : this;
    }
    private void Awake () {
        playerClickMove = playerClickMove?playerClickMove : GetComponent<M_PlayerClickMove> ();
        Gb.charactorMain = Gb.charactorMain?Gb.charactorMain : this;
    }

    public void MoveTo (Vector2 position, float maxSpeed) => playerClickMove.MoveTo (position, maxSpeed);
    public void Stop () => playerClickMove.Stop ();

    public Vector2 position { get => gameObject.transform.position; }

}