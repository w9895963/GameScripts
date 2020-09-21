using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerManager : MonoBehaviour {
    [Header ("Dependent Component")]
    public M_PlayerMove moveComp;
    public M_Gravity gravityComp;
    public Collider2D GrabBox = null;


    private void Awake () {

    }
    private void Reset () {
        moveComp = moveComp?moveComp : GetComponent<M_PlayerMove> ();
        gravityComp = gravityComp?gravityComp : GetComponent<M_Gravity> ();
    }


    public void MoveTo (Vector2 position) => moveComp.MoveTo (position);
    public void Move (Vector2 direction, float speed = -1) {
        if (speed != -1) {
            moveComp.maxSpeed = speed;
        }
        moveComp.Move (direction);
    }
    public void Stop () => moveComp.Stop ();

    public void ReverseGravity () {
        gravityComp.SetGravityDirection (-gravityComp.GetGravity ());
    }


}



namespace Global {
    public static class MainCharactorCtrl {
        public static DataClass data = new DataClass ();
        public class DataClass {
            public Vector2 Position {
                get => GameObject.FindObjectOfType<M_PlayerManager> ().transform.position.ToVector2 ();
            }
            public Vector2 Gravity {
                get => GameObject.FindObjectOfType<M_PlayerManager> ().GetComponent<M_Gravity> ().GetGravity ();
            }
        }
    }

    
}