using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Global {
    public static class Find {
        private static GameObject player;
        private static GameObject cursor;

        public static GameObject Player => FindObj<PlayerMnager> (ref player);
        public static PlayerMnager PlayerComp => Player.GetComponent<PlayerMnager> ();

        public static GameObject Cursor => FindObj<M_Cursor> (ref cursor);
        public static M_Cursor CursorComp => Cursor.GetComponent<M_Cursor> ();


        private static GameObject FindObj<T> (ref GameObject refField) where T : MonoBehaviour {
            if (refField == null) {
                refField = GameObject.FindObjectOfType<T> ().gameObject;
            }
            return refField;
        }

    }
}