using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_MainCharacter : MonoBehaviour {
    public M_PlayerMove moveComp;
    public M_Gravity gravityComp;
    public Vector2 gravity;
    public Collider2D GrabBox;


    private void Awake () {
        gravity = gravityComp.GetGravity ();
    }




}



namespace Global {
    public static class MainCharacter {
        public static Ctrl_MainCharacter Comp => GameObject.FindObjectOfType<Ctrl_MainCharacter> ();
        public static void ReverseGravity () {
            Comp.gravityComp.SetGravityDirection (-Comp.gravityComp.GetGravity ());
        }
    }
}