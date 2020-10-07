using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_MainCharacter : MonoBehaviour {
    public M_PlayerMove moveComp;
    public M_Gravity gravityComp;
    public Collider2D GrabBox;


    private void Awake () { }




}



namespace Global {
    public static class MainCharacter {
        private static Ctrl_MainCharacter comp;
        public static Ctrl_MainCharacter Comp {
            get {
                if (!comp) {
                    comp = GameObject.FindObjectOfType<Ctrl_MainCharacter> ();
                }
                return comp;
            }
        }
        public static void ReverseGravity () {
            Comp.gravityComp.SetGravityDirection (-Comp.gravityComp.GetGravity ());
        }
        public static Vector2 Gravity => Comp.gravityComp.GetGravity ();
    }
}